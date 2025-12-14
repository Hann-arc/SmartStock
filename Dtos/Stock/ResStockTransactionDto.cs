using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Dtos.Stock
{
    public class ResStockTransactionDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}