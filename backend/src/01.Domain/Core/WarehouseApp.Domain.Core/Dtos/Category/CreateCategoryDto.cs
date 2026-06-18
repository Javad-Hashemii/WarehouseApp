using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public int UserId { get; set; }
        public IFormFile? ImageFile{ get; set; }
    }
}
