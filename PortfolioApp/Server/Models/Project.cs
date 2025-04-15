using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Server.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string ThumbnailUrl { get; set; } = string.Empty;
        
        public string DemoUrl { get; set; } = string.Empty;
        
        public string SourceCodeUrl { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsPublic { get; set; } = true;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
        
        public virtual ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
        
        public virtual ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();
    }

    public class ProjectTechnology
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        
        [ForeignKey("TechnologyId")]
        public int TechnologyId { get; set; }
        
        public virtual Project Project { get; set; } = null!;
        
        public virtual Technology Technology { get; set; } = null!;
    }

    public class Technology
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public string IconUrl { get; set; } = string.Empty;
        
        public virtual ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
    }

    public class ProjectImage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        
        public string Caption { get; set; } = string.Empty;
        
        public int SortOrder { get; set; }
        
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        
        public virtual Project Project { get; set; } = null!;
    }
}