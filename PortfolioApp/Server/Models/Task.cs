using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Server.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        public DateTime? DueDate { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
    }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done,
        Archived
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public bool IsAllDay { get; set; }
        
        public string Location { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Color { get; set; } = "#1a73e8"; // Default blue
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        public virtual ApplicationUser User { get; set; } = null!;
    }
}