using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Data;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Where(c => !c.IsDeleted).AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<Category?> GetByIdIncludingDeletedAsync(Guid id)
        {
           return await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
        }

        public async Task RestoreAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return;

            if (category.IsDeleted)
            {
                category.IsDeleted = false;
                await _context.SaveChangesAsync();
            }

        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return;

            if (!category.IsDeleted)
            {
                category.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}