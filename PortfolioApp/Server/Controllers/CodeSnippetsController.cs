using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Server.Data;
using PortfolioApp.Server.Models;
using PortfolioApp.Shared.Models;
using System.Security.Claims;

namespace PortfolioApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeSnippetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CodeSnippetsController> _logger;

        public CodeSnippetsController(ApplicationDbContext context, ILogger<CodeSnippetsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/codesnippets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeSnippetDto>>> GetCodeSnippets([FromQuery] bool includePrivate = false, [FromQuery] string? tag = null, [FromQuery] string? language = null)
        {
            try
            {
                IQueryable<CodeSnippet> query = _context.CodeSnippets
                    .Include(c => c.CodeSnippetTags)
                    .ThenInclude(ct => ct.Tag)
                    .AsQueryable();

                // Filter by tag if provided
                if (!string.IsNullOrEmpty(tag))
                {
                    query = query.Where(c => c.CodeSnippetTags.Any(ct => ct.Tag.Name.ToLower() == tag.ToLower()));
                }

                // Filter by language if provided
                if (!string.IsNullOrEmpty(language))
                {
                    query = query.Where(c => c.Language.ToLower() == language.ToLower());
                }

                // Public snippets can be viewed by anyone
                if (!includePrivate || !User.Identity.IsAuthenticated)
                {
                    query = query.Where(c => c.IsPublic);
                }
                else
                {
                    // For authenticated users, include their private snippets
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    query = query.Where(c => c.IsPublic || c.UserId == userId);
                }

                var snippets = await query
                    .OrderByDescending(c => c.CreatedOn)
                    .ToListAsync();
                
                return Ok(snippets.Select(c => new CodeSnippetDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Code = c.Code,
                    Language = c.Language,
                    IsPublic = c.IsPublic,
                    GitHubUrl = c.GitHubUrl,
                    CreatedOn = c.CreatedOn,
                    LastModified = c.LastModified,
                    Tags = c.CodeSnippetTags.Select(ct => new TagDto
                    {
                        Id = ct.Tag.Id,
                        Name = ct.Tag.Name
                    }).ToList()
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving code snippets");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/codesnippets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeSnippetDto>> GetCodeSnippet(int id)
        {
            try
            {
                var snippet = await _context.CodeSnippets
                    .Include(c => c.CodeSnippetTags)
                    .ThenInclude(ct => ct.Tag)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (snippet == null)
                {
                    return NotFound();
                }

                // Check if user can access private snippet
                if (!snippet.IsPublic && (!User.Identity.IsAuthenticated || snippet.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
                {
                    return Forbid();
                }

                return new CodeSnippetDto
                {
                    Id = snippet.Id,
                    Title = snippet.Title,
                    Description = snippet.Description,
                    Code = snippet.Code,
                    Language = snippet.Language,
                    IsPublic = snippet.IsPublic,
                    GitHubUrl = snippet.GitHubUrl,
                    CreatedOn = snippet.CreatedOn,
                    LastModified = snippet.LastModified,
                    Tags = snippet.CodeSnippetTags.Select(ct => new TagDto
                    {
                        Id = ct.Tag.Id,
                        Name = ct.Tag.Name
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving code snippet with id {SnippetId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/codesnippets
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CodeSnippetDto>> CreateCodeSnippet([FromBody] CreateCodeSnippetDto snippetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var snippet = new CodeSnippet
                {
                    Title = snippetDto.Title,
                    Description = snippetDto.Description,
                    Code = snippetDto.Code,
                    Language = snippetDto.Language,
                    IsPublic = snippetDto.IsPublic,
                    GitHubUrl = snippetDto.GitHubUrl,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.CodeSnippets.Add(snippet);
                await _context.SaveChangesAsync();

                // Process tags
                if (snippetDto.Tags != null && snippetDto.Tags.Any())
                {
                    foreach (var tagName in snippetDto.Tags)
                    {
                        // Check if tag exists
                        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
                        if (tag == null)
                        {
                            // Create new tag
                            tag = new Tag { Name = tagName };
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }

                        // Add tag to snippet
                        _context.CodeSnippetTags.Add(new CodeSnippetTag
                        {
                            CodeSnippetId = snippet.Id,
                            TagId = tag.Id
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction(nameof(GetCodeSnippet), new { id = snippet.Id }, new CodeSnippetDto
                {
                    Id = snippet.Id,
                    Title = snippet.Title,
                    Description = snippet.Description,
                    Code = snippet.Code,
                    Language = snippet.Language,
                    IsPublic = snippet.IsPublic,
                    GitHubUrl = snippet.GitHubUrl,
                    CreatedOn = snippet.CreatedOn,
                    LastModified = snippet.LastModified,
                    Tags = new List<TagDto>()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating code snippet");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/codesnippets/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCodeSnippet(int id, [FromBody] UpdateCodeSnippetDto snippetDto)
        {
            if (id != snippetDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var snippet = await _context.CodeSnippets.FindAsync(id);
                if (snippet == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the snippet
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (snippet.UserId != userId)
                {
                    return Forbid();
                }

                // Update snippet details
                snippet.Title = snippetDto.Title;
                snippet.Description = snippetDto.Description;
                snippet.Code = snippetDto.Code;
                snippet.Language = snippetDto.Language;
                snippet.IsPublic = snippetDto.IsPublic;
                snippet.GitHubUrl = snippetDto.GitHubUrl;
                snippet.LastModified = DateTime.UtcNow;

                // Update tags
                if (snippetDto.Tags != null)
                {
                    // Remove existing tag associations
                    var existingTags = await _context.CodeSnippetTags
                        .Where(ct => ct.CodeSnippetId == id)
                        .ToListAsync();
                    
                    _context.CodeSnippetTags.RemoveRange(existingTags);
                    
                    // Add new tag associations
                    foreach (var tagName in snippetDto.Tags)
                    {
                        // Check if tag exists
                        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
                        if (tag == null)
                        {
                            // Create new tag
                            tag = new Tag { Name = tagName };
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }

                        // Add tag to snippet
                        _context.CodeSnippetTags.Add(new CodeSnippetTag
                        {
                            CodeSnippetId = snippet.Id,
                            TagId = tag.Id
                        });
                    }
                }

                _context.Entry(snippet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating code snippet with id {SnippetId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/codesnippets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCodeSnippet(int id)
        {
            try
            {
                var snippet = await _context.CodeSnippets.FindAsync(id);
                if (snippet == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the snippet
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (snippet.UserId != userId)
                {
                    return Forbid();
                }

                _context.CodeSnippets.Remove(snippet);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting code snippet with id {SnippetId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/codesnippets/languages
        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<string>>> GetLanguages()
        {
            try
            {
                var languages = await _context.CodeSnippets
                    .Select(c => c.Language)
                    .Distinct()
                    .OrderBy(l => l)
                    .ToListAsync();

                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving code snippet languages");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/codesnippets/tags
        [HttpGet("tags")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            try
            {
                var tags = await _context.Tags
                    .OrderBy(t => t.Name)
                    .ToListAsync();

                return Ok(tags.Select(t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}