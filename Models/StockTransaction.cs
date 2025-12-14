using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.models
{
    [Table("StockTransactions")]
    public class StockTransaction
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }
        public Guid UserId { get; set; }

        public int Quantity { get; set; }

        public TransactionType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Item Item { get; set; }
        public AppUser User { get; set; }
    }

    public enum TransactionType
    {
        In,
        Out
    }
}