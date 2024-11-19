using System;
using System.Threading.Tasks;
using Task_Management_System_API_1.Entity_Models;

namespace Task_Management_System_API_1.Services
{
    public interface ITaskService
    {
        Task<Task> CreateTask(TaskViewModel model, Guid userId);  // Method to create a task and associate it with a user
        Task<Task> UpdateTask(Guid taskId, TaskViewModel model, Guid userId);  // Method to update an existing task
        Task<bool> DeleteTask(Guid taskId, Guid userId);  // Method to delete a task
    }
}
