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
                IsPublic = true,
                Author = "Panupol Sonnuam",
                ViewCount = 42
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
                IsPublic = true,
                Author = "Panupol Sonnuam",
                ViewCount = 65
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
                IsPublic = false,
                Author = "Panupol Sonnuam",
                ViewCount = 27
            },
            // Added new code snippet
            new CodeSnippet
            {
                Id = 4,
                Title = "n8n Automation Workflow",
                Description = "Sample n8n workflow for automating notification systems",
                Language = "javascript",
                Code = @"// n8n workflow configuration
{
  ""nodes"": [
    {
      ""parameters"": {
        ""rule"": {
          ""interval"": [
            {
              ""field"": ""cronExpression"",
              ""expression"": ""0 9 * * 1-5""
            }
          ]
        }
      },
      ""name"": ""Schedule Trigger"",
      ""type"": ""n8n-nodes-base.scheduleTrigger"",
      ""typeVersion"": 1,
      ""position"": [
        250,
        300
      ]
    },
    {
      ""parameters"": {
        ""method"": ""GET"",
        ""url"": ""https://api.example.com/events"",
        ""authentication"": ""predefinedCredentialType"",
        ""credentialType"": ""httpBasicAuth"",
        ""sendQuery"": true,
        ""queryParameters"": {
          ""parameters"": [
            {
              ""name"": ""date"",
              ""value"": ""={{ new Date().toISOString().split('T')[0] }}""
            }
          ]
        }
      },
      ""name"": ""HTTP Request"",
      ""type"": ""n8n-nodes-base.httpRequest"",
      ""typeVersion"": 1,
      ""position"": [
        500,
        300
      ]
    },
    {
      ""parameters"": {
        ""conditions"": {
          ""boolean"": [
            {
              ""value1"": ""={{ $json.data.length > 0 }}"",
              ""value2"": true
            }
          ]
        }
      },
      ""name"": ""IF"",
      ""type"": ""n8n-nodes-base.if"",
      ""typeVersion"": 1,
      ""position"": [
        750,
        300
      ]
    },
    {
      ""parameters"": {
        ""message"": ""=Today's Events:\\n{{ $json.data.map(item => `- ${item.title} at ${item.time}`).join('\\n') }}"",
        ""to"": ""{{ $node['HTTP Request'].json.recipients }}"",
        ""subject"": ""Today's Event Notification""
      },
      ""name"": ""Send Email"",
      ""type"": ""n8n-nodes-base.emailSend"",
      ""typeVersion"": 1,
      ""position"": [
        1000,
        200
      ]
    }
  ],
  ""connections"": {
    ""Schedule Trigger"": {
      ""main"": [
        [
          {
            ""node"": ""HTTP Request"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    },
    ""HTTP Request"": {
      ""main"": [
        [
          {
            ""node"": ""IF"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    },
    ""IF"": {
      ""main"": [
        [
          {
            ""node"": ""Send Email"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    }
  }
}",
                Tags = new List<string> { "n8n", "Automation", "Workflow", "Notification" },
                CreatedDate = DateTime.Now.AddDays(-3),
                IsPublic = true,
                Author = "Panupol Sonnuam",
                ViewCount = 19
            }
        };

        private readonly List<Project> _projects = new()
        {
            new Project
            {
                Id = 1,
                Title = "Automation System with n8n",
                Description = "Developed comprehensive automation workflows using n8n low-code platform for scheduling, notifications, and database management.",
                DetailedDescription = "Created a set of interconnected automation workflows that streamlined business processes at QUEST EDTECH. Built systems that handled scheduling of classes, student notifications, and real-time database updates. The system reduced manual administrative work by approximately 60% and improved student engagement through timely notifications.",
                ImageUrl = "/images/projects/automation.jpg",
                GitHubUrl = "https://github.com/gotppsn/automation-workflows",
                LiveDemoUrl = "",
                Technologies = new List<string> { "JavaScript", "Python", "n8n", "APIs", "Webhooks" },
                DateCompleted = new DateTime(2023, 8, 15),
                Featured = true,
                Category = "Automation",
                Challenges = new List<string> { 
                    "Integrating multiple systems with varying APIs", 
                    "Ensuring reliable error handling and recovery", 
                    "Managing complex conditional logic for different scenarios" 
                },
                Solutions = new List<string> { 
                    "Created custom nodes for specific API integrations", 
                    "Implemented robust logging and error notification system", 
                    "Designed modular workflows with reusable components" 
                }
            },
            new Project
            {
                Id = 2,
                Title = "WordPress Website Development",
                Description = "Designed and developed custom WordPress websites using Divi and Qubely themes with multilingual support and optimized performance.",
                DetailedDescription = "Led the development of the company's main educational platform, creating over 100 pages across multiple languages (Thai, English, Chinese, Japanese). Implemented custom features for course registration, student portfolio display, and interactive learning materials. Optimized site performance for global access with a focus on mobile responsiveness.",
                ImageUrl = "/images/projects/wordpress.jpg",
                GitHubUrl = "",
                LiveDemoUrl = "https://questlanguage.com",
                Technologies = new List<string> { "WordPress", "HTML/CSS", "PHP", "JavaScript", "MySQL" },
                DateCompleted = new DateTime(2022, 6, 30),
                Featured = true,
                Category = "Web Development",
                Challenges = new List<string> { 
                    "Managing multilingual content across numerous pages", 
                    "Optimizing site speed with rich media content", 
                    "Creating custom registration and user management flows" 
                },
                Solutions = new List<string> { 
                    "Implemented WPML with custom translation workflows", 
                    "Used advanced caching and image optimization techniques", 
                    "Developed custom plugins for specific business requirements" 
                }
            },
            new Project
            {
                Id = 3,
                Title = "ProjectAppStock - Flutter App",
                Description = "Mobile application for inventory management using Flutter and Firebase. Features include product listing, stock tracking, and sales management.",
                DetailedDescription = "Developed a cross-platform mobile application for inventory management using Flutter and Firebase. The app provides real-time stock tracking, barcode scanning for quick product identification, sales reporting, and user role management. This project was created as part of my university coursework and demonstrates my ability to work with modern mobile development frameworks.",
                ImageUrl = "/images/projects/flutter.jpg",
                GitHubUrl = "https://github.com/Gotppsn/ProjectAppStock-Flutter",
                LiveDemoUrl = "",
                Technologies = new List<string> { "Flutter", "Dart", "Firebase", "Firestore", "Authentication" },
                DateCompleted = new DateTime(2023, 5, 20),
                Featured = true,
                Category = "Mobile Development",
                Challenges = new List<string> { 
                    "Implementing real-time synchronization across devices", 
                    "Creating an intuitive inventory management interface", 
                    "Handling offline functionality" 
                },
                Solutions = new List<string> { 
                    "Utilized Firebase Firestore for real-time data sync", 
                    "Designed custom UI components for inventory operations", 
                    "Implemented local caching for offline access" 
                }
            },
            new Project
            {
                Id = 4,
                Title = "LLM-Powered Chatbots",
                Description = "Developed intelligent chatbots using LLM technology through Flowise platform, creating customer service and educational assistants.",
                DetailedDescription = "Implemented AI-powered chatbot solutions that enhanced customer support and educational experiences. Created specialized bots for answering frequently asked questions, guiding students through course materials, and providing personalized learning assistance. Used advanced prompt engineering to ensure accurate and helpful responses.",
                ImageUrl = "/images/projects/ai-chatbot.jpg",
                GitHubUrl = "https://github.com/gotppsn/llm-chatbots",
                LiveDemoUrl = "",
                Technologies = new List<string> { "LLM", "Flowise", "JavaScript", "Node.js", "AI" },
                DateCompleted = new DateTime(2023, 11, 15),
                Featured = false,
                Category = "AI",
                Challenges = new List<string> { 
                    "Fine-tuning language models for specific educational domains", 
                    "Managing context in multi-turn conversations", 
                    "Optimizing response accuracy and relevance" 
                },
                Solutions = new List<string> { 
                    "Created domain-specific training datasets", 
                    "Implemented advanced context management techniques", 
                    "Developed feedback loops for continuous improvement" 
                }
            },
            new Project
            {
                Id = 5,
                Title = "Metaverse Classroom Environment",
                Description = "Created immersive educational environments in Metaverse using MozillaHub with interactive elements for enhanced learning.",
                DetailedDescription = "Designed and developed virtual classroom environments in the Metaverse using MozillaHub. Created interactive learning spaces with 3D models, educational resources, and collaborative tools. The environments were used for online language classes and coding workshops, providing students with immersive learning experiences regardless of their physical location.",
                ImageUrl = "/images/projects/metaverse.jpg",
                GitHubUrl = "",
                LiveDemoUrl = "https://hubs.mozilla.com/example-classroom",
                Technologies = new List<string> { "Mozilla Hubs", "3D Modeling", "JavaScript", "WebXR", "HTML/CSS" },
                DateCompleted = new DateTime(2022, 9, 10),
                Featured = false,
                Category = "Education",
                Challenges = new List<string> { 
                    "Creating accessible 3D learning environments", 
                    "Optimizing performance for various devices", 
                    "Integrating educational content in 3D space" 
                },
                Solutions = new List<string> { 
                    "Designed with accessibility guidelines in mind", 
                    "Used progressive level of detail and asset optimization", 
                    "Created custom interactive elements for learning activities" 
                }
            },
            new Project
            {
                Id = 6,
                Title = "ET-Table Project",
                Description = "Automated laptop storage table using Arduino with motorized compartment controlled through custom C++ programming.",
                DetailedDescription = "Designed and built an innovative table with a motorized storage compartment for laptops. Programmed the Arduino controller using C++ to manage the motor, sensors, and user interface. This project won a gold medal in the software innovation and embedded systems category at a national competition. It demonstrated practical application of programming concepts in solving real-world problems.",
                ImageUrl = "/images/projects/arduino.jpg",
                GitHubUrl = "https://github.com/Gotppsn/Project_ET-Table",
                LiveDemoUrl = "",
                Technologies = new List<string> { "Arduino", "C++", "Electronics", "Embedded Systems", "3D Design" },
                DateCompleted = new DateTime(2020, 3, 15),
                Featured = false,
                Category = "Hardware",
                Challenges = new List<string> { 
                    "Integrating mechanical components with electronic controls", 
                    "Ensuring safety with moving parts", 
                    "Creating reliable sensor feedback systems" 
                },
                Solutions = new List<string> { 
                    "Developed custom PCB for controller integration", 
                    "Implemented multiple safeguards and emergency stops", 
                    "Used redundant sensors for position verification" 
                }
            }
        };

        private readonly List<Experience> _experiences = new()
        {
            new Experience
            {
                Id = 1,
                Company = "QUEST EDTECH COMPANY LIMITED",
                Position = "Full-Stack Developer",
                Description = "Led development of enterprise educational applications, automation systems, and AI implementations for this education technology startup.",
                StartDate = new DateTime(2020, 5, 1),
                EndDate = null, // Current position
                Location = "Bangkok, Thailand",
                Achievements = new List<string>
                {
                    "Created automation systems using n8n platform, reducing manual administrative work by 60%",
                    "Developed multilingual WordPress websites with over 100 pages across 4 languages",
                    "Implemented AI solutions including LLM chatbots and content generation tools",
                    "Created virtual classroom environments in Metaverse using MozillaHub",
                    "Taught coding courses and managed intern teams"
                },
                CompanyLogoUrl = "/images/companies/quest-logo.png",
                CompanyWebsite = "https://questlanguage.com",
                Technologies = new List<string> { "JavaScript", "Python", "WordPress", "n8n", "AI", "Metaverse", "HTML/CSS" }
            },
            new Experience
            {
                Id = 2,
                Company = "Green Technology Engineering Co. Ltd",
                Position = "Intern",
                Description = "Completed a 3-month internship program gaining hands-on experience in various aspects of business operations and technical support.",
                StartDate = new DateTime(2019, 3, 1),
                EndDate = new DateTime(2019, 5, 31),
                Location = "Bangkok, Thailand",
                Achievements = new List<string>
                {
                    "Designed graphics for marketing materials and company documentation",
                    "Assisted in customer meetings with supervisors",
                    "Helped with booth setup and presentation at INFOCOMM ASIA 2019 event",
                    "Supported general administrative tasks as needed"
                },
                CompanyLogoUrl = "/images/companies/green-tech-logo.png",
                CompanyWebsite = "https://greentechengineering.co.th",
                Technologies = new List<string> { "Graphic Design", "Marketing", "Customer Relations" }
            }
        };

        private readonly List<Skill> _skills = new()
        {
            // Programming
            new Skill { Id = 1, Name = "Python", ProficiencyLevel = 5, Category = "Programming", IconUrl = "/images/skills/python.png", Description = "Advanced knowledge with experience in automation, data processing, and API development" },
            new Skill { Id = 2, Name = "JavaScript", ProficiencyLevel = 5, Category = "Programming", IconUrl = "/images/skills/javascript.png", Description = "Advanced proficiency with extensive use in web development and automation workflows" },
            new Skill { Id = 3, Name = "C++", ProficiencyLevel = 3, Category = "Programming", IconUrl = "/images/skills/cpp.png", Description = "Intermediate skills used in embedded systems projects" },
            new Skill { Id = 4, Name = "Dart", ProficiencyLevel = 3, Category = "Programming", IconUrl = "/images/skills/dart.png", Description = "Intermediate knowledge for Flutter mobile app development" },
            new Skill { Id = 5, Name = "PHP", ProficiencyLevel = 2, Category = "Programming", IconUrl = "/images/skills/php.png", Description = "Basic knowledge used in WordPress theme customization" },
            new Skill { Id = 6, Name = "C#", ProficiencyLevel = 2, Category = "Programming", IconUrl = "/images/skills/csharp.png", Description = "Basic knowledge with some experience in small projects" },
            
            // Web Development
            new Skill { Id = 7, Name = "HTML/CSS", ProficiencyLevel = 4, Category = "Web Development", IconUrl = "/images/skills/html-css.png", Description = "Strong proficiency in creating responsive and accessible web pages" },
            new Skill { Id = 8, Name = "WordPress", ProficiencyLevel = 5, Category = "Web Development", IconUrl = "/images/skills/wordpress.png", Description = "Advanced skills in theme customization, plugin integration, and multilingual site development" },
            new Skill { Id = 9, Name = "Bootstrap", ProficiencyLevel = 4, Category = "Web Development", IconUrl = "/images/skills/bootstrap.png", Description = "Extensive experience creating responsive layouts and components" },
            new Skill { Id = 10, Name = "Tailwind CSS", ProficiencyLevel = 3, Category = "Web Development", IconUrl = "/images/skills/tailwind.png", Description = "Working knowledge for creating modern UI designs" },
            new Skill { Id = 11, Name = "React", ProficiencyLevel = 2, Category = "Web Development", IconUrl = "/images/skills/react.png", Description = "Basic