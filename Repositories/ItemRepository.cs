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
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.Where(x => !x.IsDeleted).Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid guid)
        {
            return await _context.Items
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == guid && !c.IsDeleted);
        }

        public async Task<Item?> GetByIdIncludeDeleteAsync(Guid guid)
        {
           return await _context.Items.Include(c => c.Category).IgnoreQueryFilters()
           .FirstOrDefaultAsync(c => c.Id == guid);
        }

        public async Task RestoreAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null) return;
            if (item.IsDeleted)
            {
                item.IsDeleted = false;
                await _context.SaveChangesAsync();
            }

        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return;

            if (!item.IsDeleted)
            {
                item.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();

        }
    }
}