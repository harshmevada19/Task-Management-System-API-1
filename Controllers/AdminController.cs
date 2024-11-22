using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management_System_API_1.Services;
using System;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    //[HttpPost("Add")]
    //public async Task<ActionResult<User>> CreateUser(User user)
    //{
    //    var createdUser = await _adminService.CreateUserAsync(user);
    //    return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
    //}

    //[HttpGet("{id}")]
    //public async Task<ActionResult<User>> GetUser(Guid id)
    //{
    //    var user = await _adminService.GetUserByIdAsync(id);
    //    if (user == null) return NotFound();
    //    return user;
    //}

    //[HttpGet("tasks")]
    //public async Task<IActionResult> GetAllTasks()
    //{
    //    var tasks = await _adminService.GetAllTasksAsync();
    //    return Ok(tasks);
    //}

    //[HttpPost("assign-task")]
    //public async Task<IActionResult> AssignTask([FromBody] TaskViewModel model)
    //{
    //    var task = await _adminService.AssignTaskAsync(model);
    //    if (task == null) return BadRequest("User not found.");
    //    return Ok(task);
    //}

    //[HttpPut("deactivate-user/{userId}")]
    //public async Task<IActionResult> DeactivateUser(Guid userId)
    //{
    //    var result = await _adminService.DeactivateUserAsync(userId);
    //    if (!result) return NotFound();
    //    return Ok();
    //}

    //[HttpDelete("delete-user/{userId}")]
    //public async Task<IActionResult> DeleteUser(Guid userId)
    //{
    //    var result = await _adminService.DeleteUserAsync(userId);
    //    if (!result) return NotFound();
    //    return NoContent();
    //}
}
