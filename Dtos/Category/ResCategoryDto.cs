using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Category
{
    public class ResCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}