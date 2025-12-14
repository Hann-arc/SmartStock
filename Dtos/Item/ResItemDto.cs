using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Item
{
    public class ResItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int MinimumThreshold { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}