using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Stock
{
    public class ReqStockInDto
    {
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}