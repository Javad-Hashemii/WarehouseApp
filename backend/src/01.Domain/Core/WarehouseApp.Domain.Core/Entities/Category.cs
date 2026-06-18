using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string ImagPath { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
