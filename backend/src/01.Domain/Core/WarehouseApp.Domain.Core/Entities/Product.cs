using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Unit { get; set; } //should Unit be an enum or what
        public bool IsConsumable { get; set; }
        public Category Category { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
