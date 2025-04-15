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
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ApplicationDbContext context, IFileStorageService fileStorage, ILogger<ProjectsController> logger)
        {
            _context = context;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromQuery] bool includePrivate = false)
        {
            try
            {
                IQueryable<Project> query = _context.Projects
                    .Include(p => p.ProjectTechnologies)
                    .ThenInclude(pt => pt.Technology)
                    .Include(p => p.ProjectImages)
                    .AsQueryable();

                // Public projects can be viewed by anyone
                if (!includePrivate || !User.Identity.IsAuthenticated)
                {
                    query = query.Where(p => p.IsPublic);
                }
                else
                {
                    // For authenticated users, include their private projects
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    query = query.Where(p => p.IsPublic || p.UserId == userId);
                }

                var projects = await query.OrderByDescending(p => p.StartDate).ToListAsync();
                
                return Ok(projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ThumbnailUrl = p.ThumbnailUrl,
                    DemoUrl = p.DemoUrl,
                    SourceCodeUrl = p.SourceCodeUrl,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsPublic = p.IsPublic,
                    Technologies = p.ProjectTechnologies.Select(pt => new TechnologyDto
                    {
                        Id = pt.Technology.Id,
                        Name = pt.Technology.Name,
                        IconUrl = pt.Technology.IconUrl
                    }).ToList(),
                    Images = p.ProjectImages.Select(pi => new ProjectImageDto
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageUrl,
                        Caption = pi.Caption,
                        SortOrder = pi.SortOrder
                    }).OrderBy(i => i.SortOrder).ToList()
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectTechnologies)
                    .ThenInclude(pt => pt.Technology)
                    .Include(p => p.ProjectImages)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                {
                    return NotFound();
                }

                // Check if user can access private project
                if (!project.IsPublic && (!User.Identity.IsAuthenticated || project.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
                {
                    return Forbid();
                }

                return new ProjectDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    ThumbnailUrl = project.ThumbnailUrl,
                    DemoUrl = project.DemoUrl,
                    SourceCodeUrl = project.SourceCodeUrl,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    IsPublic = project.IsPublic,
                    Technologies = project.ProjectTechnologies.Select(pt => new TechnologyDto
                    {
                        Id = pt.Technology.Id,
                        Name = pt.Technology.Name,
                        IconUrl = pt.Technology.IconUrl
                    }).ToList(),
                    Images = project.ProjectImages.Select(pi => new ProjectImageDto
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageUrl,
                        Caption = pi.Caption,
                        SortOrder = pi.SortOrder
                    }).OrderBy(i => i.SortOrder).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project with id {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/projects
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var project = new Project
                {
                    Title = projectDto.Title,
                    Description = projectDto.Description,
                    ThumbnailUrl = projectDto.ThumbnailUrl,
                    DemoUrl = projectDto.DemoUrl,
                    SourceCodeUrl = projectDto.SourceCodeUrl,
                    StartDate = projectDto.StartDate,
                    EndDate = projectDto.EndDate,
                    IsPublic = projectDto.IsPublic,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                // Add technologies
                if (projectDto.TechnologyIds != null && projectDto.TechnologyIds.Any())
                {
                    foreach (var techId in projectDto.TechnologyIds)
                    {
                        var tech = await _context.Technologies.FindAsync(techId);
                        if (tech != null)
                        {
                            _context.ProjectTechnologies.Add(new ProjectTechnology
                            {
                                ProjectId = project.Id,
                                TechnologyId = techId
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    ThumbnailUrl = project.ThumbnailUrl,
                    DemoUrl = project.DemoUrl,
                    SourceCodeUrl = project.SourceCodeUrl,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    IsPublic = project.IsPublic,
                    Technologies = new List<TechnologyDto>(),
                    Images = new List<ProjectImageDto>()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the project
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (project.UserId != userId)
                {
                    return Forbid();
                }

                // Update project details
                project.Title = projectDto.Title;
                project.Description = projectDto.Description;
                project.ThumbnailUrl = projectDto.ThumbnailUrl;
                project.DemoUrl = projectDto.DemoUrl;
                project.SourceCodeUrl = projectDto.SourceCodeUrl;
                project.StartDate = projectDto.StartDate;
                project.EndDate = projectDto.EndDate;
                project.IsPublic = projectDto.IsPublic;
                project.LastModified = DateTime.UtcNow;

                // Update technologies
                if (projectDto.TechnologyIds != null)
                {
                    // Remove existing technology associations
                    var existingTechs = await _context.ProjectTechnologies
                        .Where(pt => pt.ProjectId == id)
                        .ToListAsync();
                    
                    _context.ProjectTechnologies.RemoveRange(existingTechs);
                    
                    // Add new technology associations
                    foreach (var techId in projectDto.TechnologyIds)
                    {
                        _context.ProjectTechnologies.Add(new ProjectTechnology
                        {
                            ProjectId = project.Id,
                            TechnologyId = techId
                        });
                    }
                }

                _context.Entry(project).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project with id {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectImages)
                    .FirstOrDefaultAsync(p => p.Id == id);
                
                if (project == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the project
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (project.UserId != userId)
                {
                    return Forbid();
                }

                // Delete associated images from storage
                foreach (var image in project.ProjectImages)
                {
                    await _fileStorage.DeleteFileAsync(image.ImageUrl);
                }

                // Delete thumbnail if exists
                if (!string.IsNullOrEmpty(project.ThumbnailUrl))
                {
                    await _fileStorage.DeleteFileAsync(project.ThumbnailUrl);
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project with id {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/projects/5/images
        [HttpPost("{id}/images")]
        [Authorize]
        public async Task<ActionResult<ProjectImageDto>> AddProjectImage(int id, [FromForm] AddProjectImageDto imageDto)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the project
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (project.UserId != userId)
                {
                    return Forbid();
                }

                if (imageDto.Image == null || imageDto.Image.Length == 0)
                {
                    return BadRequest("No file was uploaded");
                }

                using var memoryStream = new MemoryStream();
                await imageDto.Image.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                string fileName = Path.GetFileName(imageDto.Image.FileName);
                string contentType = imageDto.Image.ContentType;

                string imageUrl = await _fileStorage.SaveFileAsync(fileData, fileName, contentType);

                // Get next sort order
                int sortOrder = 0;
                var lastImage = await _context.ProjectImages
                    .Where(pi => pi.ProjectId == id)
                    .OrderByDescending(pi => pi.SortOrder)
                    .FirstOrDefaultAsync();
                
                if (lastImage != null)
                {
                    sortOrder = lastImage.SortOrder + 1;
                }

                var projectImage = new ProjectImage
                {
                    ProjectId = id,
                    ImageUrl = imageUrl,
                    Caption = imageDto.Caption,
                    SortOrder = sortOrder
                };

                _context.ProjectImages.Add(projectImage);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectImageDto
                {
                    Id = projectImage.Id,
                    ImageUrl = projectImage.ImageUrl,
                    Caption = projectImage.Caption,
                    SortOrder = projectImage.SortOrder
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project image to project with id {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/projects/images/5
        [HttpDelete("images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProjectImage(int imageId)
        {
            try
            {
                var projectImage = await _context.ProjectImages
                    .Include(pi => pi.Project)
                    .FirstOrDefaultAsync(pi => pi.Id == imageId);
                
                if (projectImage == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the project
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (projectImage.Project.UserId != userId)
                {
                    return Forbid();
                }

                // Delete image from storage
                await _fileStorage.DeleteFileAsync(projectImage.ImageUrl);

                _context.ProjectImages.Remove(projectImage);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project image with id {ImageId}", imageId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/projects/technologies
        [HttpGet("technologies")]
        public async Task<ActionResult<IEnumerable<TechnologyDto>>> GetTechnologies()
        {
            try
            {
                var technologies = await _context.Technologies
                    .OrderBy(t => t.Name)
                    .ToListAsync();

                return Ok(technologies.Select(t => new TechnologyDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    IconUrl = t.IconUrl
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving technologies");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/projects/technologies
        [HttpPost("technologies")]
        [Authorize]
        public async Task<ActionResult<TechnologyDto>> AddTechnology([FromBody] AddTechnologyDto technologyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if technology already exists
                var existingTech = await _context.Technologies
                    .FirstOrDefaultAsync(t => t.Name.ToLower() == technologyDto.Name.ToLower());
                
                if (existingTech != null)
                {
                    return Conflict("Technology already exists");
                }

                var technology = new Technology
                {
                    Name = technologyDto.Name,
                    IconUrl = technologyDto.IconUrl
                };

                _context.Technologies.Add(technology);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTechnologies), null, new TechnologyDto
                {
                    Id = technology.Id,
                    Name = technology.Name,
                    IconUrl = technology.IconUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding technology");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}