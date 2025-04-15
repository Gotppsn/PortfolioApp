// PersonalPortfolio/Services/ProjectService.cs
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Services
{
    public class ProjectService
    {
        private readonly List<CodeSnippet> _codeSnippets = new()
        {
            new CodeSnippet
            {
                Id = 1,
                Title = "Blazor Component Lifecycle Methods",
                Description = "Common lifecycle methods used in Blazor components",
                Language = "csharp",
                Code = @"// OnInitialized - Called once when component is initialized
        protected override void OnInitialized()
        {
            // Component initialization code
        }

        // OnInitializedAsync - Async version of OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        // OnParametersSet - Called when parameters are set
        protected override void OnParametersSet()
        {
            // Handle parameter changes
        }

        // OnParametersSetAsync - Async version of OnParametersSet
        protected override async Task OnParametersSetAsync()
        {
            await LoadDataBasedOnParametersAsync();
        }

        // OnAfterRender - Called after component has rendered
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // First time component has rendered
            }
        }

        // OnAfterRenderAsync - Async version of OnAfterRender
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync(""initializeJsPlugin"");
            }
        }",
                Tags = new List<string> { "Blazor", "C#", "Lifecycle" },
                CreatedDate = DateTime.Now.AddDays(-5),
                IsPublic = true
            },
            new CodeSnippet
            {
                Id = 2,
                Title = "Tailwind CSS Flex Layout",
                Description = "Common Tailwind CSS flex layout patterns",
                Language = "html",
                Code = @"<!-- Basic flexbox container -->
        <div class=""flex"">
        <div>Item 1</div>
        <div>Item 2</div>
        <div>Item 3</div>
        </div>

        <!-- Column layout -->
        <div class=""flex flex-col"">
        <div>Item 1</div>
        <div>Item 2</div>
        <div>Item 3</div>
        </div>

        <!-- Justify content (horizontal alignment) -->
        <div class=""flex justify-start""><!-- Items aligned to start --></div>
        <div class=""flex justify-center""><!-- Items aligned to center --></div>
        <div class=""flex justify-end""><!-- Items aligned to end --></div>
        <div class=""flex justify-between""><!-- Items with space between --></div>
        <div class=""flex justify-around""><!-- Items with space around --></div>
        <div class=""flex justify-evenly""><!-- Items with equal spacing --></div>

        <!-- Align items (vertical alignment in row, horizontal in column) -->
        <div class=""flex items-start""><!-- Items aligned to start --></div>
        <div class=""flex items-center""><!-- Items aligned to center --></div>
        <div class=""flex items-end""><!-- Items aligned to end --></div>
        <div class=""flex items-stretch""><!-- Items stretched --></div>
        <div class=""flex items-baseline""><!-- Items aligned to baseline --></div>

        <!-- Responsive flex direction -->
        <div class=""flex flex-col md:flex-row"">
        <!-- Column on mobile, row on medium screens and up -->
        </div>

        <!-- Gap between items -->
        <div class=""flex gap-4""><!-- 1rem gap between items --></div>

        <!-- Grow and shrink -->
        <div class=""flex"">
        <div class=""flex-grow""><!-- Takes up remaining space --></div>
        <div class=""flex-none""><!-- Does not grow or shrink --></div>
        <div class=""flex-shrink""><!-- Can shrink if needed --></div>
        </div>",
                Tags = new List<string> { "Tailwind", "CSS", "Flexbox", "Layout" },
                CreatedDate = DateTime.Now.AddDays(-2),
                IsPublic = true
            },
            new CodeSnippet
            {
                Id = 3,
                Title = "JavaScript Fetch API",
                Description = "Examples of using the Fetch API for HTTP requests",
                Language = "javascript",
                Code = @"// Basic GET request
        fetch('https://api.example.com/data')
        .then(response => {
            if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log('Data received:', data);
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });

        // POST request with JSON body
        fetch('https://api.example.com/users', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            name: 'John Doe',
            email: 'john@example.com'
        })
        })
        .then(response => response.json())
        .then(data => console.log('User created:', data))
        .catch(error => console.error('Error:', error));

        // Using async/await
        async function fetchData() {
        try {
            const response = await fetch('https://api.example.com/data');
            if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log('Data received:', data);
            return data;
        } catch (error) {
            console.error('Fetch error:', error);
        }
        }

        // Including credentials (cookies)
        fetch('https://api.example.com/profile', {
        credentials: 'include' // Sends cookies
        })
        .then(response => response.json())
        .then(data => console.log('Profile:', data));

        // With timeout using AbortController
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 5000); // 5 second timeout

        fetch('https://api.example.com/data', {
        signal: controller.signal
        })
        .then(response => response.json())
        .then(data => {
            clearTimeout(timeoutId);
            console.log('Data:', data);
        })
        .catch(error => {
            if (error.name === 'AbortError') {
            console.log('Request timed out');
            } else {
            console.error('Fetch error:', error);
            }
        });",
                Tags = new List<string> { "JavaScript", "Fetch", "API", "AJAX" },
                CreatedDate = DateTime.Now.AddDays(-7),
                IsPublic = false
            }
        };

        public List<CodeSnippet> GetAllCodeSnippets() => _codeSnippets;
        public CodeSnippet GetCodeSnippetById(int id) => _codeSnippets.FirstOrDefault(c => c.Id == id) ?? new CodeSnippet();
        public List<CodeSnippet> GetPublicCodeSnippets() => _codeSnippets.Where(c => c.IsPublic).ToList();
        public List<string> GetAllLanguages() => _codeSnippets.Select(c => c.Language).Distinct().OrderBy(l => l).ToList();
        public List<string> GetAllTags() => _codeSnippets.SelectMany(c => c.Tags).Distinct().OrderBy(t => t).ToList();

        public List<CodeSnippet> SearchCodeSnippets(string searchTerm, string language = null, string tag = null)
        {
            var result = _codeSnippets.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                result = result.Where(c => 
                    c.Title.ToLower().Contains(searchTerm) || 
                    c.Description.ToLower().Contains(searchTerm) ||
                    c.Code.ToLower().Contains(searchTerm));
            }
            
            if (!string.IsNullOrWhiteSpace(language))
            {
                result = result.Where(c => c.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrWhiteSpace(tag))
            {
                result = result.Where(c => c.Tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase)));
            }
            
            return result.ToList();
        }
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