using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Server.Data;
using PortfolioApp.Server.Models;
using PortfolioApp.Server.Services;
using PortfolioApp.Shared.Models;
using System.Security.Claims;

namespace PortfolioApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(ApplicationDbContext context, 
                                  IFileStorageService fileStorage,
                                  ILogger<ExperienceController> logger)
        {
            _context = context;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        // GET: api/experience
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences([FromQuery] bool includePrivate = false)
        {
            try
            {
                IQueryable<Experience> query = _context.Experiences.AsQueryable();

                // Public experiences can be viewed by anyone
                if (!includePrivate || !User.Identity.IsAuthenticated)
                {
                    query = query.Where(e => e.IsPublic);
                }
                else
                {
                    // For authenticated users, include their private experiences
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    query = query.Where(e => e.IsPublic || e.UserId == userId);
                }

                var experiences = await query
                    .OrderByDescending(e => e.IsCurrentPosition)
                    .ThenByDescending(e => e.StartDate)
                    .ToListAsync();
                
                return Ok(experiences.Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    Company = e.Company,
                    Position = e.Position,
                    Description = e.Description,
                    Location = e.Location,
                    CompanyLogoUrl = e.CompanyLogoUrl,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCurrentPosition = e.IsCurrentPosition,
                    IsPublic = e.IsPublic
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving experiences");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/experience/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienceDto>> GetExperience(int id)
        {
            try
            {
                var experience = await _context.Experiences.FindAsync(id);
                if (experience == null)
                {
                    return NotFound();
                }

                // Check if user can access private experience
                if (!experience.IsPublic && (!User.Identity.IsAuthenticated || 
                    experience.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
                {
                    return Forbid();
                }

                return new ExperienceDto
                {
                    Id = experience.Id,
                    Company = experience.Company,
                    Position = experience.Position,
                    Description = experience.Description,
                    Location = experience.Location,
                    CompanyLogoUrl = experience.CompanyLogoUrl,
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate,
                    IsCurrentPosition = experience.IsCurrentPosition,
                    IsPublic = experience.IsPublic
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving experience with id {ExperienceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/experience
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ExperienceDto>> CreateExperience([FromBody] CreateExperienceDto experienceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var experience = new Experience
                {
                    Company = experienceDto.Company,
                    Position = experienceDto.Position,
                    Description = experienceDto.Description,
                    Location = experienceDto.Location,
                    CompanyLogoUrl = experienceDto.CompanyLogoUrl,
                    StartDate = experienceDto.StartDate,
                    EndDate = experienceDto.EndDate,
                    IsCurrentPosition = experienceDto.IsCurrentPosition,
                    IsPublic = experienceDto.IsPublic,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.Experiences.Add(experience);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetExperience), new { id = experience.Id }, new ExperienceDto
                {
                    Id = experience.Id,
                    Company = experience.Company,
                    Position = experience.Position,
                    Description = experience.Description,
                    Location = experience.Location,
                    CompanyLogoUrl = experience.CompanyLogoUrl,
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate,
                    IsCurrentPosition = experience.IsCurrentPosition,
                    IsPublic = experience.IsPublic
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating experience");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/experience/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateExperience(int id, [FromBody] UpdateExperienceDto experienceDto)
        {
            if (id != experienceDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var experience = await _context.Experiences.FindAsync(id);
                if (experience == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the experience
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (experience.UserId != userId)
                {
                    return Forbid();
                }

                // Update experience details
                experience.Company = experienceDto.Company;
                experience.Position = experienceDto.Position;
                experience.Description = experienceDto.Description;
                experience.Location = experienceDto.Location;
                experience.CompanyLogoUrl = experienceDto.CompanyLogoUrl;
                experience.StartDate = experienceDto.StartDate;
                experience.EndDate = experienceDto.EndDate;
                experience.IsCurrentPosition = experienceDto.IsCurrentPosition;
                experience.IsPublic = experienceDto.IsPublic;
                experience.LastModified = DateTime.UtcNow;

                _context.Entry(experience).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating experience with id {ExperienceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/experience/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            try
            {
                var experience = await _context.Experiences.FindAsync(id);
                if (experience == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the experience
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (experience.UserId != userId)
                {
                    return Forbid();
                }

                // Delete logo if exists
                if (!string.IsNullOrEmpty(experience.CompanyLogoUrl))
                {
                    await _fileStorage.DeleteFileAsync(experience.CompanyLogoUrl);
                }

                _context.Experiences.Remove(experience);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting experience with id {ExperienceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/experience/upload-logo
        [HttpPost("upload-logo")]
        [Authorize]
        public async Task<ActionResult<string>> UploadLogo([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file was uploaded");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                string fileName = Path.GetFileName(file.FileName);
                string contentType = file.ContentType;

                string logoUrl = await _fileStorage.SaveFileAsync(fileData, fileName, contentType);

                return Ok(new { logoUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading company logo");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/experience/skills
        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills([FromQuery] bool includePrivate = false)
        {
            try
            {
                IQueryable<Skill> query = _context.Skills.AsQueryable();

                // Public skills can be viewed by anyone
                if (!includePrivate || !User.Identity.IsAuthenticated)
                {
                    query = query.Where(s => s.IsPublic);
                }
                else
                {
                    // For authenticated users, include their private skills
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    query = query.Where(s => s.IsPublic || s.UserId == userId);
                }

                var skills = await query
                    .OrderBy(s => s.Category)
                    .ThenByDescending(s => s.ProficiencyLevel)
                    .ToListAsync();
                
                return Ok(skills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    ProficiencyLevel = s.ProficiencyLevel,
                    IconUrl = s.IconUrl,
                    IsPublic = s.IsPublic
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skills");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/experience/skills
        [HttpPost("skills")]
        [Authorize]
        public async Task<ActionResult<SkillDto>> CreateSkill([FromBody] CreateSkillDto skillDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var skill = new Skill
                {
                    Name = skillDto.Name,
                    Category = skillDto.Category,
                    ProficiencyLevel = skillDto.ProficiencyLevel,
                    IconUrl = skillDto.IconUrl,
                    IsPublic = skillDto.IsPublic,
                    UserId = userId
                };

                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSkills), new { }, new SkillDto
                {
                    Id = skill.Id,
                    Name = skill.Name,
                    Category = skill.Category,
                    ProficiencyLevel = skill.ProficiencyLevel,
                    IconUrl = skill.IconUrl,
                    IsPublic = skill.IsPublic
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating skill");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/experience/skills/5
        [HttpPut("skills/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] UpdateSkillDto skillDto)
        {
            if (id != skillDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var skill = await _context.Skills.FindAsync(id);
                if (skill == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the skill
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (skill.UserId != userId)
                {
                    return Forbid();
                }

                // Update skill details
                skill.Name = skillDto.Name;
                skill.Category = skillDto.Category;
                skill.ProficiencyLevel = skillDto.ProficiencyLevel;
                skill.IconUrl = skillDto.IconUrl;
                skill.IsPublic = skillDto.IsPublic;

                _context.Entry(skill).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill with id {SkillId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/experience/skills/5
        [HttpDelete("skills/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            try
            {
                var skill = await _context.Skills.FindAsync(id);
                if (skill == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the skill
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (skill.UserId != userId)
                {
                    return Forbid();
                }

                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill with id {SkillId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}