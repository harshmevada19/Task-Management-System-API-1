using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Management_System_API_1;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.ViewModels;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllTasks()
    {
        // Retrieve the current user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get tasks based on the user's role
        var tasks = User.IsInRole("Admin")
            ? _context.Tasks.AsNoTracking().ToList()
            : _context.Tasks.Where(t => t.AssignedUserId == userId).AsNoTracking().ToList();

        // Check if tasks are found
        if (tasks == null || !tasks.Any())
        {
            return NotFound("No tasks found.");
        }

        return Ok(tasks);
    }

    [HttpPost("Create")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] TaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var task = new TaskItem
        {
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            Status = model.Status,
            AssignedUserId = userId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return Ok("Task created successfully");
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (task.AssignedUserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        task.Title = model.Title;
        task.Description = model.Description;
        task.DueDate = model.DueDate;
        task.Status = model.Status;

        await _context.SaveChangesAsync();
        return Ok("Task updated successfully");
    }


    [HttpDelete("{id,Delete}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (task.AssignedUserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return Ok("Task deleted successfully");
    }
}
