using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.models
{
    [Table("Items")]
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public int MinimumThreshold { get; set; }

        public bool IsDeleted { get; set; } =false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

         public Category Category  { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}