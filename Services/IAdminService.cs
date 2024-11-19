using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task_Management_System_API_1.Services
{
    public interface IAdminService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<List<TaskViewModel>> GetAllTasksAsync();
        Task<Task> AssignTaskAsync(TaskViewModel model);
        Task<bool> DeactivateUserAsync(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
