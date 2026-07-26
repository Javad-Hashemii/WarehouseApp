using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.SubCategory;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface ISubCategoryRepository
    {
        Task<bool> CreateSubCategoryAsync(CreateSubCategoryDto createSubCategoryDto, CancellationToken ct);
        Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryDto updateSubCategory, CancellationToken ct);
        Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesAsync(CancellationToken ct);
        Task<bool> IsSubCategoryExistAsync(string categoryName, CancellationToken ct);
        Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesByCategoryId(int categoryId, CancellationToken ct);
    }
}
