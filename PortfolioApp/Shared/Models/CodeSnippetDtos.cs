using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioApp.Shared.Models
{
    public class CodeSnippetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public string GitHubUrl { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }

    public class CreateCodeSnippetDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
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
        
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class UpdateCodeSnippetDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3)]
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
        
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}