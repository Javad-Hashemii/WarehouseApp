using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Category;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface ICategoryService
    {
        Task<ResultDto<bool>> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken ct);
        Task<List<GetAllCategoriesDto>> GetCategoriesAsync(CancellationToken ct);
        Task<GetCategoryDetailDto?> GetCategoryDetail(int id, CancellationToken ct);
    }
}
