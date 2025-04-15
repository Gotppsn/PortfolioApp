using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioApp.Shared.Models
{
    public class ExperienceDto
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string CompanyLogoUrl { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentPosition { get; set; }
        public bool IsPublic { get; set; }
    }

    public class CreateExperienceDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Company { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Position { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string Location { get; set; } = string.Empty;
        
        public string CompanyLogoUrl { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentPosition { get; set; }
        
        public bool IsPublic { get; set; } = true;
    }

    public class UpdateExperienceDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Company { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Position { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string Location { get; set; } = string.Empty;
        
        public string CompanyLogoUrl { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentPosition { get; set; }
        
        public bool IsPublic { get; set; } = true;
    }

    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; }
        public string IconUrl { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
    }

    public class CreateSkillDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty;
        
        [Range(1, 10)]
        public int ProficiencyLevel { get; set; } = 5;
        
        public string IconUrl { get; set; } = string.Empty;
        
        public bool IsPublic { get; set; } = true;
    }

    public class UpdateSkillDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty;
        
        [Range(1, 10)]
        public int ProficiencyLevel { get; set; } = 5;
        
        public string IconUrl { get; set; } = string.Empty;
        
        public bool IsPublic { get; set; } = true;
    }
}