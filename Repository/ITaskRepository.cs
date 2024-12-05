namespace Task_Management_System_API_1.Repositories
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        Task<Task> CreateTaskAsync(Task task);
        Task<Task> UpdateTaskAsync(Guid id, Task updatedTask);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<Task> GetTaskByIdAsync(Guid id);
    }
}
