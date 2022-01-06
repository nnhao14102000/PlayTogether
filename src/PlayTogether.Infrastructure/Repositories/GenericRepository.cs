using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Interfaces.Repositories;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            if(await _context.SaveChangesAsync() >= 0) {
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var obj = await dbSet.FindAsync(id);
            if (obj is not null) {
                dbSet.Remove(obj);
                if(await _context.SaveChangesAsync() >= 0) {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            if (await _context.SaveChangesAsync() >= 0) {
                return true;
            }
            return false;
        }
    }
}
