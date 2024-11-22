using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;

namespace Task_Management_System_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;


        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;

        }
        [Authorize]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Role already exists");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                return Ok(new { message = "Role added successfully" });
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost("create-task")]
        public IActionResult CreateTask(TaskViewModel taskViewModel)
        {
            // Check if the user has the Admin role
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fetch RoleId(s) for the UserId from AspNetUserRoles
            //var sfdf = _applicationDbContext.UserRoles.Where(ur => ur.Username == userId).Select(ur => ur.Role).FirstOrDefaultAsync();

            var roles = _applicationDbContext.Database
                .SqlQueryRaw<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId = @p0", userId).ToString();

            //var roles = context.Database.SqlQuery<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId = @p0", userId).ToList();

            if (ModelState.IsValid)
            {
                // Step 1: Create a new Task entity from TaskViewModel
                var task = new Task
                {
                    TaskId = Guid.NewGuid(),
                    Title = taskViewModel.Title,
                    Description = taskViewModel.Description,
                    DueDate = taskViewModel.DueDate,
                    Status = taskViewModel.Status,
                    CreatedBy = taskViewModel.CreatedBy,

                };

                // Step 2: Add the Task to the database
                _applicationDbContext.Tasks.Add(task);
                _applicationDbContext.SaveChanges();

                // Step 3: Create UserTask entries for the UserIds
                if (taskViewModel.UserIds != null && taskViewModel.UserIds.Any())
                {
                    foreach (var id in taskViewModel.UserIds)
                    {
                        if (roles == "73936166-128e-483a-95fc-3ce7dfb29382")
                        {
                            var userTask = new UserTask
                            {
                                UserId = id,
                                TaskId = task.TaskId,
                            };
                            _applicationDbContext.UserTasks.Add(userTask);
                        }
                        else
                        {
                            return Unauthorized("You are not authorized to create a task.");
                        }
                    }

                    _applicationDbContext.SaveChanges();
                }
                return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, task);
            }

            return BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetTaskById(Guid id)
        {
            var task = _applicationDbContext.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

    }
}