using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Server.Models
{
    public class Experience
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Company { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
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
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
    }

    public class Skill
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty;
        
        public int ProficiencyLevel { get; set; } // 1-10
        
        public string IconUrl { get; set; } = string.Empty;
        
        public bool IsPublic { get; set; } = true;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
    }
}