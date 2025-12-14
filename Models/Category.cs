using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.models
{
    [Table("Categories")]
    public class Category
    {

        public Guid Id { get; set; }

        public  string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

       public List<Item> Items {get; set;} = new List<Item>();
    }
}