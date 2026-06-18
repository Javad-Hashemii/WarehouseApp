using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.SubCategory;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Domain.Core.Dtos.Category
{
    public class GetCategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<SubCategoriesDto> SubCategories { get; set; }
    }
}
