using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Product;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IProductService
    {
        Task<ResultDto<bool>> CreateProductAsync(CreateProductDto createProductDto , CancellationToken ct);
        Task<List<GetAllProductsDto>> GetAllProductsAsync(CancellationToken ct);
        Task<GetProductDetailDto> GetProductDetailAsync(int productId, CancellationToken ct);
    }
}
