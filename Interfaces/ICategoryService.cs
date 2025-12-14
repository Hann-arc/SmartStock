using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.Category;

namespace SmartStockAI.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<ResCategoryDto>> GetAllAsync();
        Task<ResCategoryDto?> GetByIdAsync(Guid id);
        Task<ResCategoryDto> CreateAsync(ReqCreateCategoryDto dto);
        Task<ResCategoryDto?> UpdateAsync(Guid id, ReqUpdateCategoryDto dto);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}