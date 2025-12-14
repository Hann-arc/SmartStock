using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Data;
using SmartStockAI.Helpers;
using SmartStockAI.Interfaces;
using SmartStockAI.models;


namespace SmartStockAI.Repositories
{
    public class StockTransactionRepository : IStockTransactionRepository
    {

        private readonly ApplicationDBContext _context;

        public StockTransactionRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StockTransaction trx)
        {
            await _context.StockTransactions.AddAsync(trx);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(Guid itemId)
        {
            return await _context.StockTransactions.CountAsync(t => t.ItemId == itemId);
        }

        public async Task<List<StockTransaction>> GetByItemIdAsync(Guid itemId)
        {
            return await _context.StockTransactions.Where(t => t.ItemId == itemId).Include(t => t.Item).Include(t => t.User).OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<List<StockTransaction>> GetHistoryAsync(Guid itemId, int page, int pageSize)
        {
            return await _context.StockTransactions
                .Where(t => t.ItemId == itemId)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.User)
                .ToListAsync();

        }

        public async Task<List<DailySalesData>> GetDailySalesDataAsync(Guid itemId, DateTime startDate, DateTime endDate)
        {
            return await _context.StockTransactions
                .Where(st => st.ItemId == itemId &&
                            st.Type == TransactionType.Out &&
                            st.CreatedAt >= startDate &&
                            st.CreatedAt <= endDate)
                .GroupBy(st => st.CreatedAt.Date)
                .Select(g => new DailySalesData
                {
                    Date = g.Key,
                    TotalQuantity = g.Sum(st => st.Quantity)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}