// PersonalPortfolio/Models/CodeSnippet.cs
namespace PersonalPortfolio.Models
{
    public class CodeSnippet
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool IsPublic { get; set; } = false;
        public string Author { get; set; } = "Panupol Sonnuam"; // Added to track authorship
        public int ViewCount { get; set; } = 0; // Added to track popularity
        public List<string> RelatedSnippets { get; set; } = new List<string>(); // Added for related content
    }
}