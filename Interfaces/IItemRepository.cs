using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(Guid guid);
        Task<Item?> GetByIdIncludeDeleteAsync(Guid guid);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task SoftDeleteAsync(Guid id);
        Task RestoreAsync(Guid id);
    }
}