using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Task_Management_System_API_1.Repositories;

namespace Task_Management_System_API_1.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        // Injecting IMapper into the service
        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        //public async Task<User> CreateUserAsync(User user)
        //{
        //    return await _adminRepository.CreateUserAsync(user);
        //}

        //public async Task<User> GetUserByIdAsync(Guid userId)
        //{
        //    return await _adminRepository.GetUserByIdAsync(userId);
        //}

        //public async Task<List<TaskViewModel>> GetAllTasksAsync()
        //{
        //    var tasks = await _adminRepository.GetAllTasksAsync();
        //    return _mapper.Map<List<TaskViewModel>>(tasks);
        //}

        //public async Task<Task> AssignTaskAsync(TaskViewModel model)
        //{
        //    var userExists = await _adminRepository.UserExistsAsync(model.UserId);
        //    if (!userExists) return null;

        //    var task = _mapper.Map<Task>(model);
        //    task.TaskId = Guid.NewGuid();

        //    return await _adminRepository.AssignTaskAsync(task);
        //}

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            return await _adminRepository.DeactivateUserAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            return await _adminRepository.DeleteUserAsync(userId);
        }
    }
}
