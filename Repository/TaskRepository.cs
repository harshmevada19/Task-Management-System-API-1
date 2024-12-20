using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Repositories;

public class TaskRepository : GenericRepository<Task>, ITaskRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<Task> CreateTaskAsync(Task task)
    {
        _applicationDbContext.Tasks.Add(task); 
        await _applicationDbContext.SaveChangesAsync(); 
        return task; 
    }
    public async Task<Task> UpdateTaskAsync(Guid id, Task updatedTask)
    {
        var existingTask = await _applicationDbContext.Tasks.FindAsync(id); 
        if (existingTask != null)
        {
            _applicationDbContext.Entry(existingTask).CurrentValues.SetValues(updatedTask); 
            await _applicationDbContext.SaveChangesAsync(); 
        }
        return existingTask; 
    }
           
    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task = await _applicationDbContext.Tasks.FindAsync(id); 
        if (task != null)
        {
            _applicationDbContext.Tasks.Remove(task); 
            await _applicationDbContext.SaveChangesAsync(); 
            return true; 
        }
        return false; 
    }
    public async Task<Task> GetTaskByIdAsync(Guid id)
    {
        return await _applicationDbContext.Tasks.FindAsync(id); 
    }
}
