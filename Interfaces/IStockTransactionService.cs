using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Stock
{
    public interface IStockTransactionService
    {
        Task<ResStockTransactionDto> StockInAsync(Guid userId, ReqStockInDto dto);
        Task<ResStockTransactionDto> StockOutAsync(Guid userId, ReqStockOutDto dto);
        Task<int> CountHistoryAsync(Guid itemId);
        Task<IEnumerable<ResStockTransactionDto>> GetItemHistoryAsync(Guid itemId, int page, int pageSize);
    }
}