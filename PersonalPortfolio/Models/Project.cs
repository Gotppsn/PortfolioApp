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

// PersonalPortfolio/Models/Experience.cs
namespace PersonalPortfolio.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Current => EndDate == null;
        public string Location { get; set; } = string.Empty;
        public List<string> Achievements { get; set; } = new List<string>();
    }
}

// PersonalPortfolio/Models/Skill.cs
namespace PersonalPortfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; } // 1-5 scale
        public string Category { get; set; } = string.Empty; // Frontend, Backend, Database, etc.
        public string IconUrl { get; set; } = string.Empty;
    }
}