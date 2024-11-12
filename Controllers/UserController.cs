using Microsoft.AspNetCore.Mvc;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.Data; // Assuming this is the namespace for your DbContext

namespace Task_Management_System_API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the DbContext
        public UserTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/UserTask/Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserTask([FromBody] UserTask userTask)
        {
            if (userTask == null)
            {
                return BadRequest("Invalid user task data.");
            }

            // Add the new UserTask to the context
            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();

            // Return a response with the added user task
            return CreatedAtAction(nameof(AddUserTask), new { id = userTask.UserId }, userTask);
        }
    }
}
