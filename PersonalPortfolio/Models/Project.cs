// PersonalPortfolio/Models/Project.cs
namespace PersonalPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DetailedDescription { get; set; } = string.Empty; // Added for project details page
        public string ImageUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string LiveDemoUrl { get; set; } = string.Empty;
        public List<string> Technologies { get; set; } = new List<string>();
        public DateTime DateCompleted { get; set; }
        public bool Featured { get; set; }
        public List<string> Screenshots { get; set; } = new List<string>(); // Added for multiple screenshots
        public string VideoUrl { get; set; } = string.Empty; // Added for demo video
        public string Category { get; set; } = string.Empty; // Added for categorization
        public List<string> Challenges { get; set; } = new List<string>(); // Added to describe challenges faced
        public List<string> Solutions { get; set; } = new List<string>(); // Added to describe solutions implemented
    }
}