using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Alert
{
    public class ResAlertDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Resolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByEmail { get; set; } = string.Empty;
    }
}