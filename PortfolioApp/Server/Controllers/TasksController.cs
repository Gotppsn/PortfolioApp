using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Server.Data;
using PortfolioApp.Server.Models;
using PortfolioApp.Shared.Models;
using System.Security.Claims;

namespace PortfolioApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ApplicationDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] TaskStatusFilter? status = null)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var query = _context.Tasks
                    .Where(t => t.UserId == userId)
                    .AsQueryable();

                // Filter by status if provided
                if (status.HasValue && status != TaskStatusFilter.All)
                {
                    TaskStatus taskStatus = (TaskStatus)((int)status - 1);
                    query = query.Where(t => t.Status == taskStatus);
                }

                var tasks = await query
                    .OrderBy(t => t.Status)
                    .ThenBy(t => t.DueDate)
                    .ThenByDescending(t => t.Priority)
                    .ToListAsync();
                
                return Ok(tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = (TaskStatusFilter)((int)t.Status + 1),
                    Priority = (TaskPriorityLevel)t.Priority,
                    DueDate = t.DueDate,
                    CreatedOn = t.CreatedOn,
                    LastModified = t.LastModified
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the task
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (task.UserId != userId)
                {
                    return Forbid();
                }

                return new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = (TaskStatusFilter)((int)task.Status + 1),
                    Priority = (TaskPriorityLevel)task.Priority,
                    DueDate = task.DueDate,
                    CreatedOn = task.CreatedOn,
                    LastModified = task.LastModified
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with id {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var task = new Models.Task
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    Status = (TaskStatus)((int)taskDto.Status - 1),
                    Priority = (Models.TaskPriority)taskDto.Priority,
                    DueDate = taskDto.DueDate,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = (TaskStatusFilter)((int)task.Status + 1),
                    Priority = (TaskPriorityLevel)task.Priority,
                    DueDate = task.DueDate,
                    CreatedOn = task.CreatedOn,
                    LastModified = task.LastModified
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the task
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (task.UserId != userId)
                {
                    return Forbid();
                }

                // Update task details
                task.Title = taskDto.Title;
                task.Description = taskDto.Description;
                task.Status = (TaskStatus)((int)taskDto.Status - 1);
                task.Priority = (Models.TaskPriority)taskDto.Priority;
                task.DueDate = taskDto.DueDate;
                task.LastModified = DateTime.UtcNow;

                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with id {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the task
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (task.UserId != userId)
                {
                    return Forbid();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with id {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tasks/calendar
        [HttpGet("calendar")]
        public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetCalendarEvents([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var query = _context.CalendarEvents
                    .Where(e => e.UserId == userId)
                    .AsQueryable();

                // Filter by date range if provided
                if (start.HasValue)
                {
                    query = query.Where(e => e.StartTime >= start.Value || e.EndTime >= start.Value);
                }
                
                if (end.HasValue)
                {
                    query = query.Where(e => e.StartTime <= end.Value);
                }

                var events = await query
                    .OrderBy(e => e.StartTime)
                    .ToListAsync();
                
                return Ok(events.Select(e => new CalendarEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    IsAllDay = e.IsAllDay,
                    Location = e.Location,
                    Color = e.Color
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calendar events");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tasks/calendar
        [HttpPost("calendar")]
        public async Task<ActionResult<CalendarEventDto>> CreateCalendarEvent([FromBody] CreateCalendarEventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var calendarEvent = new CalendarEvent
                {
                    Title = eventDto.Title,
                    Description = eventDto.Description,
                    StartTime = eventDto.StartTime,
                    EndTime = eventDto.EndTime,
                    IsAllDay = eventDto.IsAllDay,
                    Location = eventDto.Location,
                    Color = eventDto.Color,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.CalendarEvents.Add(calendarEvent);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCalendarEvent), new { id = calendarEvent.Id }, new CalendarEventDto
                {
                    Id = calendarEvent.Id,
                    Title = calendarEvent.Title,
                    Description = calendarEvent.Description,
                    StartTime = calendarEvent.StartTime,
                    EndTime = calendarEvent.EndTime,
                    IsAllDay = calendarEvent.IsAllDay,
                    Location = calendarEvent.Location,
                    Color = calendarEvent.Color
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating calendar event");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tasks/calendar/5
        [HttpGet("calendar/{id}")]
        public async Task<ActionResult<CalendarEventDto>> GetCalendarEvent(int id)
        {
            try
            {
                var calendarEvent = await _context.CalendarEvents.FindAsync(id);
                if (calendarEvent == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the event
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (calendarEvent.UserId != userId)
                {
                    return Forbid();
                }

                return new CalendarEventDto
                {
                    Id = calendarEvent.Id,
                    Title = calendarEvent.Title,
                    Description = calendarEvent.Description,
                    StartTime = calendarEvent.StartTime,
                    EndTime = calendarEvent.EndTime,
                    IsAllDay = calendarEvent.IsAllDay,
                    Location = calendarEvent.Location,
                    Color = calendarEvent.Color
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calendar event with id {EventId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/tasks/calendar/5
        [HttpPut("calendar/{id}")]
        public async Task<IActionResult> UpdateCalendarEvent(int id, [FromBody] UpdateCalendarEventDto eventDto)
        {
            if (id != eventDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var calendarEvent = await _context.CalendarEvents.FindAsync(id);
                if (calendarEvent == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the event
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (calendarEvent.UserId != userId)
                {
                    return Forbid();
                }

                // Update event details
                calendarEvent.Title = eventDto.Title;
                calendarEvent.Description = eventDto.Description;
                calendarEvent.StartTime = eventDto.StartTime;
                calendarEvent.EndTime = eventDto.EndTime;
                calendarEvent.IsAllDay = eventDto.IsAllDay;
                calendarEvent.Location = eventDto.Location;
                calendarEvent.Color = eventDto.Color;
                calendarEvent.LastModified = DateTime.UtcNow;

                _context.Entry(calendarEvent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating calendar event with id {EventId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/tasks/calendar/5
        [HttpDelete("calendar/{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            try
            {
                var calendarEvent = await _context.CalendarEvents.FindAsync(id);
                if (calendarEvent == null)
                {
                    return NotFound();
                }

                // Authorization: Check if user owns the event
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (calendarEvent.UserId != userId)
                {
                    return Forbid();
                }

                _context.CalendarEvents.Remove(calendarEvent);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting calendar event with id {EventId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}