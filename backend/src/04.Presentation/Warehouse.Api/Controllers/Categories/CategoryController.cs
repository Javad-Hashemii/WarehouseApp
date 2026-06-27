using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseApp.Application.Services;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.Category;

namespace Warehouse.Api.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto, CancellationToken ct)
        {
            var result = await _categoryService.CreateCategoryAsync(dto, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken ct)
        {
            var categories = await _categoryService.GetCategoriesAsync(ct);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryDetail(int id, CancellationToken ct)
        {
            var category = await _categoryService.GetCategoryDetail(id, ct);

            if (category == null)
                return NotFound("دسته‌بندی مورد نظر یافت نشد."); // Returns 404 if the ID doesn't exist

            return Ok(category);
        }
    }
}
