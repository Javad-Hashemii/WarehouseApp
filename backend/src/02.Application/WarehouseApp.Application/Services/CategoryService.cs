using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Category;

namespace WarehouseApp.Application.Services
{
    public class CategoryService(ICategoryRepository categoryRepository,IFileService fileService) : ICategoryService
    {
        public async Task<ResultDto<bool>> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken ct)
        {
            try
            {
                bool isNameTaken= await categoryRepository.IsExistByNameAsync(createCategoryDto.Name, ct);
                if(isNameTaken)
                {
                    return ResultDto<bool>.Failure("نام دسته بندی تکراری است!");
                }

                var uploadResult = UploadCategoryImage(createCategoryDto);
                if(!uploadResult.IsSuccess)
                {
                    return ResultDto<bool>.Failure(uploadResult.Message);
                }

                var result = await categoryRepository.CreateCategoryAsync(createCategoryDto, ct);
                if (!result)
                {
                    return ResultDto<bool>.Failure("خطا در ایجاد دسته بندی!");

                }
                return ResultDto<bool>.Success("دسته بندی جدید ثبت شد.");
            }
            catch(Exception ex)
            {
                return ResultDto<bool>.Failure("خطایی در سیستم رخ داده است.");

            }

        }

        public async Task<List<GetAllCategoriesDto>> GetCategoriesAsync(CancellationToken ct)
        {
            return await categoryRepository.GetCategoriesAsync(ct);
        }

        public async Task<GetCategoryDetailDto?> GetCategoryDetail(int id, CancellationToken ct)
        {
            return await categoryRepository.GetCategoryDetailAsync(id, ct);
        }

        public async Task<ResultDto<bool>> Update(UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken)
        {

            var imageResult = await ImageUpdate(updateCategoryDto, cancellationToken);
            if (!imageResult.IsSuccess)
            {
                return ResultDto<bool>.Failure(imageResult.Message);
            }

            var result = await categoryRepository.UpdateCategoryAsync(updateCategoryDto, cancellationToken);
            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ویرایش دسته بندی!");
            }
            return ResultDto<bool>.Success("دسته بندی ویرایش شد.");

        }

        private ResultDto<bool> UploadCategoryImage(CreateCategoryDto categoryDto)
        {
            // 1. Check if the file is actually sent
            if (categoryDto.ImageFile == null || categoryDto.ImageFile.Length == 0)
            {
                // Pass a message first, then the boolean data
                return ResultDto<bool>.Success("تصویری برای آپلود ارسال نشد، اما عملیات ادامه یافت.", true);
            }

            try
            {
                var filePath = fileService.Upload(categoryDto.ImageFile, "Category");

                if (string.IsNullOrEmpty(filePath))
                {
                    // Message first, bool defaults to false
                    return ResultDto<bool>.Failure("آپلود تصویر ناموفق بود!");
                }

                categoryDto.ImagePath = filePath;

                // Pass a success message first, then the boolean 'true'
                return ResultDto<bool>.Success("آپلود با موفقیت انجام شد.", true);
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Failure("خطایی در هنگام آپلود تصویر رخ داد.");
            }
        }
        private async Task<ResultDto<bool>> ImageUpdate(UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            if (dto.ImageFile is not null)
            {

                var currentImageUrl = await categoryRepository.GetImageUrl(dto.CategoryId, cancellationToken);

                var newImagePath = fileService.ReplaceFile(currentImageUrl, dto.ImageFile, "categories");
                if (!string.IsNullOrEmpty(newImagePath))
                {
                    dto.ImagePath = newImagePath;
                    return ResultDto<bool>.Success("");
                }
                else
                {
                    return ResultDto<bool>.Failure("جایگزینی تصویر ناموفق بود!");
                }
            }
            else
            {
                return ResultDto<bool>.Success("");
            }
        }
    }
}
