using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Category;
using WarehouseApp.Domain.Core.Dtos.SubCategory;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class CategoryRepository(AppDbContext _context) : ICategoryRepository
    {
        public async Task<bool> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken ct)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                ImagPath = createCategoryDto.ImagePath,
                CreatedAt = DateTime.Now,
                CreatedByUserId = createCategoryDto.UserId,
            };
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<List<GetAllCategoriesDto>> GetCategoriesAsync(CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new GetAllCategoriesDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagPath
                })
                .ToListAsync(ct);
        }

        public async Task<GetCategoryDetailDto?> GetCategoryDetailAsync(int id, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new GetCategoryDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagPath,
                    SubCategories = c.SubCategories.Select(sc => new SubCategoriesDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        ImagePath = sc.ImagePath
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsExistByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name, ct);
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto updateCategory, CancellationToken ct)
        {
            var result = _context.Categories
                .Where(c => c.Id == updateCategory.CategoryId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Name, updateCategory.Name)
                    .SetProperty(c => c.ImagPath, updateCategory.ImagePath)
                    .SetProperty(c => c.UpdatedAt, DateTime.Now)
                    .SetProperty(c => c.UpdatedByUserId, updateCategory.UpdatedBy), ct);

            return await result > 0;
        }

        public async Task<string?> GetImageUrl(int id, CancellationToken cancellationToken) //change the  naming of the property in category entity to ImagePath
        {

            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.ImagPath)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
