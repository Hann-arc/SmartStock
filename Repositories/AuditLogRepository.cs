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
    public class AuditLogRepository : IAuditLogRepository
    {

        private readonly ApplicationDBContext _context;

        public AuditLogRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<AuditLog>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, Guid? userId = null, string? entityType = null, string? action = null, int page = 1, int pageSize = 10)
        {
            var query = _context.AuditLogs.Include(a => a.User).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.TimeStemp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.TimeStemp <= endDate.Value);

            if (userId.HasValue)
                query = query.Where(a => a.UserId == userId.Value);

            if (!string.IsNullOrEmpty(entityType))
                query = query.Where(a => a.EntityType == entityType);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(a => a.Action == action);


            var totalCount = await query.CountAsync();
            var items = await query
            .OrderByDescending(a => a.TimeStemp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return new PagedResult<AuditLog>(items, totalCount, page, pageSize);
        }
    }
}