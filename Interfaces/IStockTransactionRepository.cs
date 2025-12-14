using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Helpers;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface IStockTransactionRepository
    {
        Task AddAsync(StockTransaction trx);
        Task<List<StockTransaction>> GetByItemIdAsync(Guid itemId);
        Task<List<StockTransaction>> GetHistoryAsync(Guid itemId, int page, int pageSize);
        Task<List<DailySalesData>> GetDailySalesDataAsync(Guid itemId, DateTime startDate, DateTime endDate);
        Task<int> CountAsync(Guid itemId);
    }
}