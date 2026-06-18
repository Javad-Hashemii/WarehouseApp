using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int CategoryId { get; set; }
        public int UpdatedBy { get; set; }
    }
}
