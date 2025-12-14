using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.models
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string EntityType { get; set; } = string.Empty;

        public Guid EntityId { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public DateTime TimeStemp { get; set; } = DateTime.Now;

        public AppUser User { get; set; }

    }
}