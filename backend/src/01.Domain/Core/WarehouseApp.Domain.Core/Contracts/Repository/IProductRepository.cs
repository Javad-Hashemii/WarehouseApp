using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Product;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IProductRepository
    {
        Task<bool> CreateProductAsync(CreateProductDto createProductDto,CancellationToken ct);
        Task<List<GetAllProductsDto>>GetAllProductsAsync(CancellationToken ct);
        Task<GetProductDetailDto>GetProductDetailAsync(int productId,CancellationToken ct);
        Task<bool> IsProductExistAsync(string productName, CancellationToken ct);
        Task<bool> AllProductsExistsAsync(List<int> productsIds, CancellationToken ct);
        Task<bool> IsProductExistByIdAsync(int productId, CancellationToken ct);
        Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken ct);

        Task<bool> SoftDeleteProductAsync(int productId, int deletedByUserId, CancellationToken ct);

        Task<PaginatedResult<GetAllProductsDto>>SearchProductsAsync(string? search,int page,int pageSize,int? categoryId,int? subCategoryId, CancellationToken ct);


    }
}
