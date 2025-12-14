using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.models
{
    [Table("Alerts")]
    public class Alert
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }

        public Guid CreatedById { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool Resolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Item Item { get; set; }

        public AppUser CreatedBy { get; set; }
    }
}