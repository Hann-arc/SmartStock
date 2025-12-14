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
    public class AlertRepository : IAlertRepository

    {
        private readonly ApplicationDBContext _context;

        public AlertRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Alert alert)
        {
            await _context.Alerts.AddAsync(alert);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {
            return await _context.Alerts
                .Include(a => a.Item)
                .Include(a => a.CreatedBy)
                .Where(a => !a.Resolved)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Alert?> GetByIdAsync(Guid id)
        {
            return await _context.Alerts
                .Include(a => a.Item)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Resolved);
        }

        public async Task<Alert?> GetActiveAlertByItemIdAsync(Guid itemId)
        {
            return await _context.Alerts
                .FirstOrDefaultAsync(a => a.ItemId == itemId && !a.Resolved);
        }

        public async Task UpdateAsync(Alert alert)
        {
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Alert>> GetLowStockAlertsAsync()
        {
            return await _context.Alerts
                .Where(a => !a.Resolved)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetResolvedAlertsAsync()
        {
            return await _context.Alerts
                .Include(a => a.Item)
                .Include(a => a.CreatedBy)
                .Where(a => a.Resolved)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task ResolveAlertAsync(Guid id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert != null)
            {
                alert.Resolved = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}