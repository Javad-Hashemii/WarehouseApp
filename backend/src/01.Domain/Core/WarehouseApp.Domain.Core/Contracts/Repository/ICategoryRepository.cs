using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Category;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface ICategoryRepository
    {
        Task<bool> CreateCategoryAsync(CreateCategoryDto createCategoryDto , CancellationToken ct);
        Task<List<GetAllCategoriesDto>> GetCategoriesAsync(CancellationToken ct);
        Task<GetCategoryDetailDto?> GetCategoryDetailAsync(int id, CancellationToken ct);
        Task<bool> IsExistByNameAsync(string name, CancellationToken ct);
        Task<string?> GetImageUrl(int id, CancellationToken cancellationToken); //change the  naming of the property in category entity to ImagePath
        Task<bool> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto, CancellationToken ct);
    }
}
