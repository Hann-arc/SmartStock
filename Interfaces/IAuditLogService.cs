using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.AuditLog;

namespace SmartStockAI.Interfaces
{
    public interface IAuditLogService
    {
         Task LogActionAsync<T>(
            Guid userId, 
            string action, 
            string entityType, 
            Guid entityId,
            T oldValues = default,
            T newValues = default
        );

        Task<PagedResult<ResAuditLogDto>> GetLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? userId = null,
            string? entityType = null,
            string? action = null,
            int page = 1,
            int pageSize = 10
        );
    }
}