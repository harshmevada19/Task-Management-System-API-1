using Microsoft.EntityFrameworkCore;
using Task_Management_System_API_1.Data;

namespace Task_Management_System_API_1.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext  = applicationDbContext;
            _dbSet = applicationDbContext.Set<T>();
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var resultObj = _dbSet.Update(entity).Entity;
            var result = await _applicationDbContext.SaveChangesAsync();
            return resultObj;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            var result = await _applicationDbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
