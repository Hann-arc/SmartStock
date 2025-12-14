using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> GetByIdIncludingDeletedAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task SoftDeleteAsync(Guid id);
        Task RestoreAsync(Guid id);
    }
}