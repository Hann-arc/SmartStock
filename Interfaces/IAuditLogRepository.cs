using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
        Task<PagedResult<AuditLog>> GetLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? userId = null,
            string? entityType = null,
            string? action = null,
            int page = 1,
            int pageSize = 10
        );
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PagedResult(List<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}