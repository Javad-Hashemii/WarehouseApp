using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.SubCategory;

namespace WarehouseApp.Application.Services
{
    public class SubCategoryService(ISubCategoryRepository _subCategoryRepository) : ISubCategoryService
    {
        public async Task<ResultDto<bool>> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto, CancellationToken ct)
        {
            bool isSubCategoryExist = await _subCategoryRepository.IsSubCategoryExistAsync(createSubCategoryDto.Name, ct);
            if (isSubCategoryExist)
            {
                return ResultDto<bool>.Failure("زیر دسته بندی با این نام قبلاً ایجاد شده است", false);
            }
            else
            {
                var result = await _subCategoryRepository.CreateSubCategoryAsync(createSubCategoryDto, ct);
                if(result)
                {
                    return ResultDto<bool>.Success("زیر دسته بندی با موفقیت ایجاد شد", true);
                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در ایجاد زیر دسته بندی", false);
                }
            }
        }

        public async Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesAsync(CancellationToken ct)
        {
            return await _subCategoryRepository.GetAllSubCategoriesAsync(ct);
        }

        public async Task<ICollection<GetAllSubCateogoriesDto>> GetAllSubCategoriesByCategoryId(int categoryId, CancellationToken ct)
        {
            return await _subCategoryRepository.GetAllSubCategoriesByCategoryId(categoryId, ct);
        }

        

        public async Task<ResultDto<bool>> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto, CancellationToken ct)
        {
            var result = await _subCategoryRepository.UpdateSubCategoryAsync(updateSubCategoryDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("زیر دسته بندی با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی زیر دسته بندی", false);
            }
        }


    }
}
