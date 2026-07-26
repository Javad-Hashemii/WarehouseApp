using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.SubCategory;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface ISubCategoryService
    {
        Task<ResultDto<bool>> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto, CancellationToken ct);
        Task<ResultDto<bool>> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto, CancellationToken ct);
        Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesAsync(CancellationToken ct);
        Task<ICollection<GetAllSubCateogoriesDto>>GetAllSubCategoriesByCategoryId(int categoryId, CancellationToken ct);
    }
}
