using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Category
{
    public class ReqUpdateCategoryDto
    {
        [StringLength(100)]
        public string? Name { get; set; }
    }
}