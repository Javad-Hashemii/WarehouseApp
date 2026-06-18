using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ImagePath { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
