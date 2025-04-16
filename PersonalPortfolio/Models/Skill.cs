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
        public string? Description { get; set; } // Added field for more detail
    }
}