using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.Item;

namespace SmartStockAI.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ResItemDto>> GetAllAsync();
        Task<ResItemDto?> GetByIdAsync(Guid id);
        Task<ResItemDto> CreateAsync(ReqCreateItemDto dto, Guid currentUserId);
        Task<ResItemDto?> UpdateAsync(Guid id, ReqUpdateItemDto dto, Guid currentUserId);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}