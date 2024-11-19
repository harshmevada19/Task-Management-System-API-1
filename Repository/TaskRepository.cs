using Microsoft.EntityFrameworkCore;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;

namespace Task_Management_System_API_1.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Task> CreateTask(Task task)
        {
            try
            {
                // Add the task to the database
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();

                return task;  // Return the created task
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("An error occurred while saving the task to the database.", ex);
            }
        }


        public async Task<Task> GetTaskById(Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<IEnumerable<Task>> GetTasksByUserId(Guid userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Task> UpdateTask(Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(Guid taskId, Guid userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId && t.UserId == userId);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
