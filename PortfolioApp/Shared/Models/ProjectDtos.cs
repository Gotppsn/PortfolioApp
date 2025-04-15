using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PortfolioApp.Shared.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string DemoUrl { get; set; } = string.Empty;
        public string SourceCodeUrl { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPublic { get; set; }
        public List<TechnologyDto> Technologies { get; set; } = new List<TechnologyDto>();
        public List<ProjectImageDto> Images { get; set; } = new List<ProjectImageDto>();
    }

    public class CreateProjectDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string ThumbnailUrl { get; set; } = string.Empty;
        
        public string DemoUrl { get; set; } = string.Empty;
        
        public string SourceCodeUrl { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsPublic { get; set; } = true;
        
        public List<int> TechnologyIds { get; set; } = new List<int>();
    }

    public class UpdateProjectDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string ThumbnailUrl { get; set; } = string.Empty;
        
        public string DemoUrl { get; set; } = string.Empty;
        
        public string SourceCodeUrl { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsPublic { get; set; } = true;
        
        public List<int> TechnologyIds { get; set; } = new List<int>();
    }

    public class ProjectImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class AddProjectImageDto
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
        
        public string Caption { get; set; } = string.Empty;
    }

    public class TechnologyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class AddTechnologyDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        
        public string IconUrl { get; set; } = string.Empty;
    }
}