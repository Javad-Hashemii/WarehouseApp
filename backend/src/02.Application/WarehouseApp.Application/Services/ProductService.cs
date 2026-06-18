using Microsoft.OpenApi;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Product;

namespace WarehouseApp.Application.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<ResultDto<bool>> CreateProductAsync(CreateProductDto createProductDto, CancellationToken ct)
        {
            bool isProductExist = await productRepository.IsProductExistAsync(createProductDto.ProductName, ct);
            if (isProductExist)
            {
                return ResultDto<bool>.Failure("محصول قبلاً ثبت شده است.");
            }
            else
            {
                var result = await productRepository.CreateProductAsync(createProductDto, ct);
                if (result)
                {
                    return ResultDto<bool>.Success("محصول با موفقیت ایجاد شد.", true);
                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در ایجاد محصول.");
                }
            }
        }

        public async Task<List<GetAllProductsDto>> GetAllProductsAsync(CancellationToken ct)
        {
            return await productRepository.GetAllProductsAsync(ct);
        }

        public async Task<GetProductDetailDto> GetProductDetailAsync(int productId, CancellationToken ct)
        {
            var product= await productRepository.GetProductDetailAsync(productId, ct);
            if(product == null)
            {
                throw new Exception("محصول مورد نظر یافت نشد."); //exception??? or result dto
            }
            else
            {
                return product;
            }
        }

        //update method??
    }
}

