// PersonalPortfolio/Services/ProjectService.cs
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Services
{
    public class ProjectService
    {
        private readonly List<Project> _projects = new()
        {
            new Project
            {
                Id = 1,
                Title = "E-Commerce Platform",
                Description = "Full-stack e-commerce application with product catalog, shopping cart, and payment integration.",
                ImageUrl = "images/projects/ecommerce.jpg",
                GitHubUrl = "https://github.com/yourusername/ecommerce",
                LiveDemoUrl = "https://ecommerce-demo.example.com",
                Technologies = new List<string> { "C#", "ASP.NET Core", "SQL Server", "React", "Azure" },
                DateCompleted = new DateTime(2023, 6, 15),
                Featured = true
            },
            new Project
            {
                Id = 2,
                Title = "Task Management System",
                Description = "Kanban-style task management application with real-time updates and team collaboration features.",
                ImageUrl = "images/projects/taskmanager.jpg",
                GitHubUrl = "https://github.com/yourusername/taskmanager",
                LiveDemoUrl = "https://taskmanager-demo.example.com",
                Technologies = new List<string> { "C#", "Blazor", "Entity Framework", "SQL Server", "SignalR" },
                DateCompleted = new DateTime(2023, 9, 20),
                Featured = true
            },
            // Add more projects as needed
        };

        private readonly List<Experience> _experiences = new()
        {
            new Experience
            {
                Id = 1,
                Company = "Tech Solutions Inc.",
                Position = "Senior Software Developer",
                Description = "Lead development of enterprise web applications using .NET Core and Azure.",
                StartDate = new DateTime(2021, 3, 1),
                EndDate = null, // Current position
                Location = "Seattle, WA",
                Achievements = new List<string>
                {
                    "Reduced application loading time by 40% through code optimization",
                    "Implemented CI/CD pipeline reducing deployment time by 60%",
                    "Led team of 5 developers in successful product launch"
                }
            },
            new Experience
            {
                Id = 2,
                Company = "DataSystems LLC",
                Position = "Software Developer",
                Description = "Developed and maintained data-intensive applications.",
                StartDate = new DateTime(2018, 6, 1),
                EndDate = new DateTime(2021, 2, 28),
                Location = "Portland, OR",
                Achievements = new List<string>
                {
                    "Created data visualization dashboard for business analytics",
                    "Optimized database queries improving performance by 35%",
                    "Integrated third-party APIs for enhanced functionality"
                }
            }
        };

        private readonly List<Skill> _skills = new()
        {
            new Skill { Id = 1, Name = "C#", ProficiencyLevel = 5, Category = "Backend", IconUrl = "images/skills/csharp.png" },
            new Skill { Id = 2, Name = "ASP.NET Core", ProficiencyLevel = 5, Category = "Backend", IconUrl = "images/skills/aspnet.png" },
            new Skill { Id = 3, Name = "SQL Server", ProficiencyLevel = 4, Category = "Database", IconUrl = "images/skills/sql.png" },
            new Skill { Id = 4, Name = "JavaScript", ProficiencyLevel = 4, Category = "Frontend", IconUrl = "images/skills/js.png" },
            new Skill { Id = 5, Name = "Blazor", ProficiencyLevel = 4, Category = "Frontend", IconUrl = "images/skills/blazor.png" },
            new Skill { Id = 6, Name = "React", ProficiencyLevel = 3, Category = "Frontend", IconUrl = "images/skills/react.png" },
            new Skill { Id = 7, Name = "Azure", ProficiencyLevel = 4, Category = "DevOps", IconUrl = "images/skills/azure.png" },
            new Skill { Id = 8, Name = "Docker", ProficiencyLevel = 3, Category = "DevOps", IconUrl = "images/skills/docker.png" },
        };

        public List<Project> GetAllProjects() => _projects;
        public Project GetProjectById(int id) => _projects.FirstOrDefault(p => p.Id == id) ?? new Project();
        public List<Project> GetFeaturedProjects() => _projects.Where(p => p.Featured).ToList();
        
        public List<Experience> GetAllExperiences() => _experiences.OrderByDescending(e => e.StartDate).ToList();
        
        public List<Skill> GetAllSkills() => _skills;
        public List<Skill> GetSkillsByCategory(string category) => _skills.Where(s => s.Category == category).ToList();
        public List<string> GetSkillCategories() => _skills.Select(s => s.Category).Distinct().ToList();
    }
}