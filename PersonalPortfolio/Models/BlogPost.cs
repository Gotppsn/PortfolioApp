// PersonalPortfolio/Models/BlogPost.cs
using System.Text.RegularExpressions;

namespace PersonalPortfolio.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsFeatured { get; set; }
        
        // Changed from computed property to regular property with backing field
        private int _readingTimeMinutes;
        public int ReadingTimeMinutes 
        { 
            get => _readingTimeMinutes > 0 ? _readingTimeMinutes : CalculateReadingTime();
            set => _readingTimeMinutes = value;
        }
        
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorImageUrl { get; set; } = string.Empty;
        
        public string Slug 
        { 
            get 
            {
                if (string.IsNullOrEmpty(Title)) return string.Empty;
                
                // Remove special characters
                string slug = Regex.Replace(Title.ToLower(), @"[^a-z0-9\s-]", "");
                // Replace spaces with hyphens
                slug = Regex.Replace(slug, @"\s+", "-");
                // Remove consecutive hyphens
                slug = Regex.Replace(slug, @"-+", "-");
                // Remove leading and trailing hyphens
                slug = slug.Trim('-');
                
                return slug;
            }
        }
        
        private int CalculateReadingTime()
        {
            if (string.IsNullOrEmpty(Content)) return 0;
            
            // Average reading speed is about 200-250 words per minute
            const int wordsPerMinute = 225;
            
            // Count words in content
            int wordCount = Content.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            
            // Calculate reading time in minutes
            int readingTime = (int)Math.Ceiling((double)wordCount / wordsPerMinute);
            
            // Return at least 1 minute
            return Math.Max(1, readingTime);
        }
    }
}