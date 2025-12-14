using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SmartStockAI.models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public UserRole Role { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public ICollection<Alert> CreatedAlerts { get; set; } = new List<Alert>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }

    public enum UserRole
    {
        Admin,
        Staff
    }
}