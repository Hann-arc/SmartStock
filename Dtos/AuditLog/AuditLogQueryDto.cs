using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.AuditLog
{
    public class AuditLogQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? UserId { get; set; }
        public string? EntityType { get; set; }
        public string? Action { get; set; }
    }
}