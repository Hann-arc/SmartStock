using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Item
{
    public class ReqCreateItemDto
    {
        [Required , StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid CategoryId { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int MinimumThreshold { get; set; }
    }
}