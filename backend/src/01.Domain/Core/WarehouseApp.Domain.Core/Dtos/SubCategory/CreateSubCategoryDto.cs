using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.SubCategory
{
    public class CreateSubCategoryDto
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public int CategoryId { get; set; }
    }
}
