using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task_Management_System_API_1.Repositories
{
    public interface IAdminRepository
    {
        //Task<User> CreateUserAsync(User user);
        //Task<User> GetUserByIdAsync(Guid userId);
        //Task<List<Task>> GetAllTasksAsync();
        //Task<bool> UserExistsAsync(Guid userId);
        Task<Task> AssignTaskAsync(Task task);
        Task<bool> DeactivateUserAsync(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
