using Task_Management_System_API_1.Entity_Models;

public interface ITaskService
{
    Task<Task> CreateTaskAsync(TaskViewModel taskViewModel);
    Task<Task> UpdateTaskAsync(Guid id, TaskViewModel taskViewModel);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<Task> GetTaskByIdAsync(Guid id);
}
