using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.Alert;


namespace SmartStockAI.Interfaces
{
    public interface IAlertService
    {
        Task CheckLowStockItemsAsync();
        Task<IEnumerable<ResAlertDto>> GetActiveAlertsAsync();
        Task<ResAlertDto> ResolveAlertAsync(ReqResolveAlertDto dto);
        Task<bool> IsItemBelowThresholdAsync(Guid itemId);
    }
}