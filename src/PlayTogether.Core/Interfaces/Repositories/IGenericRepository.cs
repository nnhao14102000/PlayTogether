using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<bool> CreateAsync(T entity);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> UpdateAsync(T entity);
    }
}
