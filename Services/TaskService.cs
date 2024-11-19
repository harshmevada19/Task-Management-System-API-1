using System;
using System.Threading.Tasks;
using AutoMapper;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.Repositories;

namespace Task_Management_System_API_1.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public TaskService(ITaskRepository taskRepository, IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Task> CreateTask(TaskViewModel model, Guid userId)
        {
            try
            {
                // Map the TaskViewModel to Task entity
                var task = _mapper.Map<Task>(model);
                task.TaskId = Guid.NewGuid(); // Assign a new TaskId

                // Call AddTaskAsync in the repository to add the task to the database
                var createdTask = await _taskRepository.CreateTask(task);

                // Create UserTask to associate the user with the created task
                var userTask = new UserTask
                {
                    UserId = userId,
                    TaskId = createdTask.TaskId // Using the TaskId from the created task
                };

                // Add the UserTask association directly to the UserTasks table
                _applicationDbContext.UserTasks.Add(userTask);
                await _applicationDbContext.SaveChangesAsync();

                return createdTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the task and associating it with the user.", ex);
            }
        }



        public async Task<Task> UpdateTask(Guid taskId, TaskViewModel model, Guid userId)
        {
            var task = await _taskRepository.GetTaskById(taskId);

            if (task == null || task.UserId != userId)
                throw new Exception("Task not found or access denied.");

            // Use AutoMapper to update the task with the model data
            _mapper.Map(model, task);

            return await _taskRepository.UpdateTask(task);
        }

        public async Task<bool> DeleteTask(Guid taskId, Guid userId)
        {
            return await _taskRepository.DeleteTask(taskId, userId);
        }
    }
}
