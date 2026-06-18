using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.SubCategory;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class SubCategoryRepository(AppDbContext _context) : ISubCategoryRepository
    {
        public async Task<bool> CreateSubCategoryAsync(CreateSubCategoryDto createSubCategoryDto, CancellationToken ct)
        {
            var subCategory = new SubCategory
            {
                Name = createSubCategoryDto.Name,
                CategoryId = createSubCategoryDto.CategoryId
            };
            _context.SubCategories.Add(subCategory);
            return await _context.SaveChangesAsync(ct) > 0;

        }

        public async Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesAsync(CancellationToken ct)
        {
            return await _context.SubCategories
                .Select(sc => new GetAllSubCateogoriesDto
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    CategoryId = sc.CategoryId
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryDto updateSubCategory, CancellationToken ct)
        {
            var result = _context.SubCategories
                .Where(sc => sc.Id == updateSubCategory.Id)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(sc => sc.Name, updateSubCategory.Name)
                .SetProperty(sc => sc.CategoryId, updateSubCategory.CategoryId), ct);
            return await result > 0;
        }

        public async Task<bool> IsSubCategoryExistAsync(string categoryName, CancellationToken ct)
        {
            return await _context.SubCategories.AnyAsync(sc => sc.Name == categoryName, ct);
        }
    }
}
