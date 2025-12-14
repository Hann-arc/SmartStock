using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Stock
{
    public class StockHistoryQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}