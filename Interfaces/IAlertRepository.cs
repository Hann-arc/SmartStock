using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface IAlertRepository
    {
        Task AddAsync(Alert alert);
        Task UpdateAsync(Alert alert);
        Task<Alert?> GetByIdAsync(Guid id);
        Task<Alert?> GetActiveAlertByItemIdAsync(Guid itemId);
        Task<IEnumerable<Alert>> GetActiveAlertsAsync();
        Task<IEnumerable<Alert>> GetResolvedAlertsAsync();
        Task ResolveAlertAsync(Guid id);
        Task<List<Alert>> GetLowStockAlertsAsync();
    }
}