using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Product;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class ProductRepository(AppDbContext _context,SiteSettings siteSettings) : IProductRepository
    {
        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto, CancellationToken ct)
        {
            var product = new Product
            {
                Name = createProductDto.ProductName,
                CategoryId = createProductDto.CategoryId,
                SubCategoryId = createProductDto.SubCategoryId,
                Unit = createProductDto.Unit,
                IsConsumable = createProductDto.IsConsumable
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<List<GetAllProductsDto>> GetAllProductsAsync(CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetAllProductsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    CategoryName=p.Category.Name,
                    SubCategoryId = p.SubCategoryId,
                    SubCategoryName = p.SubCategory!.Name,
                    Unit = p.Unit,
                    IsConsumable = p.IsConsumable
                }
                )
                .ToListAsync(ct);
        }

        public async Task<GetProductDetailDto> GetProductDetailAsync(int productId, CancellationToken ct)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, ct);

            if (product == null)
                throw new InvalidOperationException("Product not found");

            return new GetProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Unit = product.Unit,
                IsConsumable = product.IsConsumable
            };
        }

        public async Task<bool> IsProductExistAsync(string productName, CancellationToken ct)
        {
            return await _context.Products.AnyAsync(p => p.Name == productName, ct);
        }

        public async Task<bool> IsProductExistByIdAsync(int productId, CancellationToken ct)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId, ct);
        }

        public async Task<bool> AllProductsExistsAsync(List<int> productsIds, CancellationToken ct)
        {
            int existingCount = await _context.Products
                .CountAsync(p => productsIds.Contains(p.Id), ct);
            return existingCount == productsIds.Count;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken ct)
        {
            var result = await _context.Products
                .Where(p => p.Id == updateProductDto.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, updateProductDto.Name)
                    .SetProperty(p => p.CategoryId, updateProductDto.CategoryId)
                    .SetProperty(p => p.SubCategoryId, updateProductDto.SubCategoryId)
                    .SetProperty(p => p.Unit, updateProductDto.Unit)
                    .SetProperty(p => p.IsConsumable, updateProductDto.IsConsumable)
                    .SetProperty(p=>p.UpdatedAt, DateTime.UtcNow)
                    ,
                    ct);
            return result > 0;
        }

        public async Task<bool> SoftDeleteProductAsync(int productId,int deletedByUserId,CancellationToken ct)
        {
            int affectedRows = await _context.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.IsDeleted, true)
                    .SetProperty(p => p.DeletedAt, DateTime.UtcNow)
                    .SetProperty(p => p.DeletedByUserId, deletedByUserId),
                    ct);

            return affectedRows > 0;
        }

        public async Task<PaginatedResult<GetAllProductsDto>> SearchProductsAsync(string? search, int page, int pageSize, int? categoryId, int? subCategoryId, CancellationToken ct)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted);


            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search));
            }


            if (categoryId.HasValue)
            {
                query = query.Where(p =>
                    p.CategoryId == categoryId.Value);
            }


            if (subCategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.SubCategoryId == subCategoryId.Value);
            }


            var totalCount = await query.CountAsync(ct);


            var products = await query
                .OrderBy(p => p.Name)

                .Skip((page - 1) * pageSize)

                .Take(pageSize)

                .Select(p => new GetAllProductsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Unit = p.Unit,
                    CategoryName = p.Category.Name,
                    SubCategoryName = p.SubCategory != null
                        ? p.SubCategory.Name
                        : null
                })

                .ToListAsync(ct);


            return new PaginatedResult<GetAllProductsDto>
            {
                Items = products,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}

