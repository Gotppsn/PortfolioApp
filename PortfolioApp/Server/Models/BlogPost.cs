using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Server.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Summary { get; set; } = string.Empty;
        
        public string FeaturedImageUrl { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;
        
        public bool IsPublic { get; set; } = true;
        
        public bool IsFeatured { get; set; }
        
        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
        
        public virtual BlogCategory? Category { get; set; }
        
        public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
        
        public virtual ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
    }

    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }

    public class BlogPostTag
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("BlogPostId")]
        public int BlogPostId { get; set; }
        
        [ForeignKey("TagId")]
        public int TagId { get; set; }
        
        public virtual BlogPost BlogPost { get; set; } = null!;
        
        public virtual Tag Tag { get; set; } = null!;
    }

    public class BlogComment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string AuthorName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string AuthorEmail { get; set; } = string.Empty;
        
        public string AuthorWebsite { get; set; } = string.Empty;
        
        public bool IsApproved { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("BlogPostId")]
        public int BlogPostId { get; set; }
        
        public virtual BlogPost BlogPost { get; set; } = null!;
    }
}