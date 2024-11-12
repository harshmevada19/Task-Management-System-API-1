using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Management_System_API_1;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("users")]
    [Authorize]
    public IActionResult GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        return Ok(users);
    }

    [HttpPost("assign-task")]
    [Authorize]
    public async Task<IActionResult> AssignTaskToUser(int taskId, string userId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return NotFound();

        task.AssignedUserId = userId;
        await _context.SaveChangesAsync();
        return Ok("Task assigned to user");
    }

    [HttpDelete("deactivate-user/{userId}")]
    [Authorize]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;
        await _userManager.UpdateAsync(user);

        return Ok("User deactivated successfully");
    }
}
