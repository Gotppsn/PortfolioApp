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
        public string CompanyLogoUrl { get; set; } = string.Empty; // Added field for company logo
        public string CompanyWebsite { get; set; } = string.Empty; // Added field for company website
        public List<string> Technologies { get; set; } = new List<string>(); // Added field for technologies used
    }
}