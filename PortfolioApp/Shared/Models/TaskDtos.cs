using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioApp.Shared.Models
{
    public enum TaskStatusFilter
    {
        All = 0,
        ToDo = 1,
        InProgress = 2,
        Done = 3,
        Archived = 4
    }

    public enum TaskPriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatusFilter Status { get; set; }
        public TaskPriorityLevel Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class CreateTaskDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public TaskStatusFilter Status { get; set; } = TaskStatusFilter.ToDo;
        
        public TaskPriorityLevel Priority { get; set; } = TaskPriorityLevel.Medium;
        
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public TaskStatusFilter Status { get; set; }
        
        public TaskPriorityLevel Priority { get; set; }
        
        public DateTime? DueDate { get; set; }
    }

    public class CalendarEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Color { get; set; } = "#1a73e8";
    }

    public class CreateCalendarEventDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public bool IsAllDay { get; set; }
        
        public string Location { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Color { get; set; } = "#1a73e8";
    }

    public class UpdateCalendarEventDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public bool IsAllDay { get; set; }
        
        public string Location { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Color { get; set; } = "#1a73e8";
    }
}