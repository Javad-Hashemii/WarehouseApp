using Microsoft.OpenApi;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Product;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Application.Services
{
    public class ProductService(IProductRepository productRepository,SiteSettings siteSettings) : IProductService
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

        public async Task<ResultDto<bool>>UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken ct)
        {
            bool isProductExist = await productRepository.IsProductExistByIdAsync(updateProductDto.Id, ct);
            if (!isProductExist)
            {
                return ResultDto<bool>.Failure("محصول مورد نظر یافت نشد.");
            }
            else
            {
                var result = await productRepository.UpdateProductAsync(updateProductDto, ct);
                if (result)
                {
                    return ResultDto<bool>.Success("محصول با موفقیت بروزرسانی شد.", true);
                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در بروزرسانی محصول.");
                }
            }
        }

        public async Task<ResultDto<bool>> AreProductsExist(List<int> productIds, CancellationToken ct)
        {
            bool allProductsExist = await productRepository.AllProductsExistsAsync(productIds, ct);
            if (allProductsExist)
            {
                return ResultDto<bool>.Success("همه محصولات موجود هستند.", true);
            }
            else
            {
                return ResultDto<bool>.Failure("برخی از محصولات یافت نشدند.");
            }
        }

        public async Task<ResultDto<bool>> SoftDeleteProductAsync(int productId,int deletedByUserId,CancellationToken ct)
        {
            bool productExists = await productRepository
                .IsProductExistByIdAsync(productId, ct);

            if (!productExists)
            {
                return ResultDto<bool>.Failure("محصول مورد نظر یافت نشد.");
            }

            bool result = await productRepository.SoftDeleteProductAsync(productId,deletedByUserId,ct);

            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در حذف محصول.");
            }

            return ResultDto<bool>.Success( "محصول با موفقیت حذف شد.",true);
        }

        public async Task<PaginatedResult<GetAllProductsDto>> SearchProductsAsync(
            string? search,
            int? categoryId,
            int? subCategoryId,
            int page,
            CancellationToken ct)
        {
            var pageSize = siteSettings
                .WarehouseConfigurations
                .PaginationSettings
                .ProductPageSize;

            return await productRepository.SearchProductsAsync(
                search,
                page,
                pageSize,
                categoryId,
                subCategoryId,
                ct);
        }

    }
}


