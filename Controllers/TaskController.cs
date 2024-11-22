using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.Services;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ApplicationDbContext _applicationDbContext;

    public TaskController(ITaskService taskService, ApplicationDbContext applicationDbContext)
    {
        _taskService = taskService;
        _applicationDbContext = applicationDbContext;
    }

    //[HttpPost]
    //[Authorize]
    //public async Task<IActionResult> CreateTask([FromBody] TaskViewModel model)
    //{
        
    //    try
    //    {
    //        if (_applicationDbContext == null)
    //        {
    //            return StatusCode(500, "Database context is not available.");
    //        }

    //        var userId = model.CreatedBy;
    //        var userRole = await _applicationDbContext.Users
    //        .Where(u => u.UserId == userId) 
    //        .Select(u => u.Role)           
    //        .FirstOrDefaultAsync();

    //        // Check if the user has a role assigned and it matches "Admin" (RoleId = 1)
    //        if (userRole == 1) // Check if the role is "Admin"
    //        {
    //            var task = await _taskService.CreateTask(model, userId);
    //            //var userTask = new UserTask
    //            //{
    //            //    UserId = userId,
    //            //    TaskId = task.TaskId // Assuming task has TaskId
    //            //};
    //            //_applicationDbContext.UserTasks.Add(userTask);
    //            //await _applicationDbContext.SaveChangesAsync();
    //            return Ok(task);
    //        }
    //        else if (userRole == 2) // Check if the role is "User"
    //        {
    //            return Unauthorized("Users are not authorized to create a task.");
    //        }
    //        else
    //        {
    //            return Unauthorized("You are not authorized to create a task.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Internal server error: {ex.Message}");
    //    }
    //}

    //[HttpPut("{taskId}")]
    //public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] TaskViewModel model)
    //{
    //    var jh = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (jh == null)
    //        return Unauthorized();

    //    try
    //    {
    //        var updatedTask = await _taskService.UpdateTask(taskId, model, Guid.Parse(jh));
    //        return Ok(updatedTask);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Internal server error: {ex.Message}");
    //    }
    //}

    //[HttpDelete("{taskId}")]
    //public async Task<IActionResult> DeleteTask(Guid taskId)
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (userId == null)
    //        return Unauthorized();

    //    var success = await _taskService.DeleteTask(taskId, Guid.Parse(userId));

    //    if (!success)
    //        return NotFound();

    //    return NoContent();
    //}
}
