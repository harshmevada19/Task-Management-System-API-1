using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ClaimsAuthorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("create-task")]
    public async Task<IActionResult> CreateTask(TaskViewModel taskViewModel)
    {
        if (ModelState.IsValid)
        {    
            var response = await _taskService.CreateTaskAsync(taskViewModel);
            return Ok(response);
        }
        return BadRequest(ModelState);
    }

    [HttpPut("update-task/{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, TaskViewModel taskViewModel)
    {
        if (ModelState.IsValid)
        {
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            var updatedTask = await _taskService.UpdateTaskAsync(id, taskViewModel);
            return Ok(updatedTask);
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("delete-task/{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var existingTask = await _taskService.GetTaskByIdAsync(id);
        if (existingTask == null)
        {
            return NotFound($"Task with ID {id} not found.");
        }

        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }
}
