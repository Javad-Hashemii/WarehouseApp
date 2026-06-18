using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Unit { get; set; }
        public bool IsConsumable { get; set; }

    }
}
