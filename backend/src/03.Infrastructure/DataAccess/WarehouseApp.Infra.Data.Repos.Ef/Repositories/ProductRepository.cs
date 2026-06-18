using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Product;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class ProductRepository(AppDbContext _context) : IProductRepository
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
                    SubCategoryId = p.SubCategoryId,
                    Unit = p.Unit,
                    IsConsumable = p.IsConsumable
                })
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
    }
}

