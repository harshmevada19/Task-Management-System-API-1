using AutoMapper;
using Task_Management_System_API_1.Repositories;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    public async Task<Task> CreateTaskAsync(TaskViewModel taskViewModel)
    {
        try
        {
            var task = _mapper.Map<Task>(taskViewModel); 
            return await _taskRepository.CreateAsync(task);
            //return await _taskRepository.CreateTaskAsync(task);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateTaskAsync: {ex.Message}");
            throw;
        }
    }
    public async Task<Task> UpdateTaskAsync(Guid id, TaskViewModel taskViewModel)
    {
        try
        {
            var task = _mapper.Map<Task>(taskViewModel); 
            return await _taskRepository.UpdateAsync(task); 
            //return await _taskRepository.UpdateTaskAsync(id, task); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateTaskAsync: {ex.Message}");
            throw;
        }
    }
    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        try
        {
            return await _taskRepository.DeleteAsync(id);
            //return await _taskRepository.DeleteTaskAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteTaskAsync: {ex.Message}");
            throw;
        }
    }
    public async Task<Task> GetTaskByIdAsync(Guid id)
    {
        try
        {
            return await _taskRepository.GetByIdAsync(id);
            //return await _taskRepository.GetTaskByIdAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetTaskByIdAsync: {ex.Message}");
            throw;
        }
    }
}
