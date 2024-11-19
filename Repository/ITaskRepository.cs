using Task_Management_System_API_1.Entity_Models;

namespace Task_Management_System_API_1.Repositories
{
    public interface ITaskRepository
    {
        Task<Task> CreateTask(Task task);
        Task<Task> GetTaskById(Guid taskId);
        Task<IEnumerable<Task>> GetTasksByUserId(Guid userId);
        Task<Task> UpdateTask(Task task);
        Task<bool> DeleteTask(Guid taskId, Guid userId);

    }
}
