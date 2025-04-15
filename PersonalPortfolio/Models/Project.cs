// PersonalPortfolio/Models/Project.cs
namespace PersonalPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string LiveDemoUrl { get; set; } = string.Empty;
        public List<string> Technologies { get; set; } = new List<string>();
        public DateTime DateCompleted { get; set; }
        public bool Featured { get; set; }
    }
}