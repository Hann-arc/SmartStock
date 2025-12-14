using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.AuditLog
{
    public class ResAuditLogDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
    }
}