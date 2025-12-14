using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Item
{
    public class ReqUpdateItemDto
    {
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int? Stock { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumThreshold { get; set; }
    }
}