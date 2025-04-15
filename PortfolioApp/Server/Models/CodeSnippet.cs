using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Server.Models
{
    public class CodeSnippet
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Language { get; set; } = string.Empty;
        
        public bool IsPublic { get; set; } = true;
        
        public string GitHubUrl { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
        
        public virtual ICollection<CodeSnippetTag> CodeSnippetTags { get; set; } = new List<CodeSnippetTag>();
    }

    public class CodeSnippetTag
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("CodeSnippetId")]
        public int CodeSnippetId { get; set; }
        
        [ForeignKey("TagId")]
        public int TagId { get; set; }
        
        public virtual CodeSnippet CodeSnippet { get; set; } = null!;
        
        public virtual Tag Tag { get; set; } = null!;
    }

    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        
        public virtual ICollection<CodeSnippetTag> CodeSnippetTags { get; set; } = new List<CodeSnippetTag>();
        public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
    }
}