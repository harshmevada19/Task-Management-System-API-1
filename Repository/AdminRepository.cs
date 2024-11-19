using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_Management_System_API_1.Data;

namespace Task_Management_System_API_1.Repositories
{
    public class AdminRepository : GenericRepository<User>, IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<Task>> GetAllTasksAsync()
        {
            return await _context.Tasks.Include(t => t.User).ToListAsync();
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.UserId == userId);
        }

        public async Task<Task> AssignTaskAsync(Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
