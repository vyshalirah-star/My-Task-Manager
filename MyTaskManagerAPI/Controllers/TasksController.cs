using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyTaskManagerAPI.Data;
using MyTaskManagerAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyTaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly MyTaskManagerContext _context;

        public TasksController(MyTaskManagerContext context)
        {
            _context = context;
        }

        // GET: all details
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Category)
                .ToListAsync();
            return Ok(tasks);
        }

        // GET: taskId & categoryId as inputs
        [HttpGet("{taskId}/{categoryId}")]
        public async Task<IActionResult> GetTask(int taskId, int categoryId)
        {
            var task = await _context.Tasks
                        .Include(t => t.Category)
                        .FirstOrDefaultAsync(t => t.TaskId == taskId && t.CategoryId == categoryId);

            if (task == null) return NotFound();
            return Ok(task);
        }

        // GET: categoryId & taskId & isActive
        [HttpGet("filter")]
        public async Task<IActionResult> GetTasksByArchiveStatus(
            [FromQuery, BindRequired] int categoryId,   // Required
            [FromQuery, BindRequired] int taskId,       // Required
            [FromQuery, BindRequired] bool isActive = true) // true → active, false → archived
        {
            var tasks = await _context.Tasks
                .Where(t => t.CategoryId == categoryId 
                        && t.TaskId == taskId 
                        && t.IsArchived == !isActive) // If active=true, we need IsArchived=false
                .ToListAsync();

            if (!tasks.Any())
                return NotFound("No matching tasks found.");

            return Ok(tasks);
        }

        // POST : insertion
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Models.Task task)
        {
            if (Enum.IsDefined(typeof(TaskPriority), task.Priority))
            {
                task.Priority = (TaskPriority)task.Priority;
            }  // convert int → enum
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { taskId = task.TaskId, categoryId = task.CategoryId }, task);
        }

        // PUT: updates all except - taskId & categoryId
        [HttpPut("{taskId}/{categoryId}")]
        public async Task<IActionResult> UpdateTask(int taskId, int categoryId, [FromBody] Models.Task task)
        {
            if (taskId != task.TaskId || categoryId != task.CategoryId) return BadRequest();

            task.UpdatedAt = DateTime.UtcNow;      // Update timestamp
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: taskId &categoryId as inputs, update particular fields
        [HttpPatch("complete/{taskId}/{categoryId}")]
        public async Task<IActionResult> MarkComplete(int taskId, int categoryId)
        {
            var task = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.TaskId == taskId && t.CategoryId == categoryId);

            if (task == null) return NotFound();

            task.IsCompleted = true;               // Mark as done
            task.IsArchived = true;                // Mark as archived
            task.UpdatedAt = DateTime.UtcNow;      // Update timestamp

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: based on taskId &categoryId 
        [HttpDelete("{taskId}/{categoryId}")]
        public async Task<IActionResult> DeleteTask(int taskId,  int categoryId)
        {
            var task = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.TaskId == taskId && t.CategoryId == categoryId);

            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
