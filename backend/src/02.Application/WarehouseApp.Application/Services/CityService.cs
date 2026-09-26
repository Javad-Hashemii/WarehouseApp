using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.City;

namespace WarehouseApp.Application.Services
{
    public class CityService(ICityRepository cityRepository) : ICityService
    {
        public async Task<ResultDto<bool>> CreateCity(CreateCityDto createCityDto, CancellationToken ct)
        {
            bool isCityExist = await cityRepository.IsCityExist(createCityDto.Name, ct);
            if (isCityExist)
            {
                return ResultDto<bool>.Failure("نام دسته بندی تکراری است.");
            }
            var result = await cityRepository.CreateCity(createCityDto, ct);
            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ایجاد شهر.");
            }
            else
            {
                return ResultDto<bool>.Success("شهر با موفقیت ایجاد شد.", true);
            }
        }

        public async Task<List<GetAllCitiesDto>> GetAllCities(CancellationToken ct)
        {
            return await cityRepository.GetAllCities(ct);
        }

        public async Task<ResultDto<bool>> UpdateCity(UpdateCityDto updateCityDto, CancellationToken ct)
        {
            if (updateCityDto == null)
            {
                return ResultDto<bool>.Failure("اطلاعات ورودی نامعتبر است."); // "Invalid input data."
            }

            // Notice we use currentUserId (from the secure token) instead of the DTO
            var result = await cityRepository.UpdateCity(updateCityDto.Id, updateCityDto.UserId, updateCityDto.Name, ct);

            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در بروزرسانی شهر. ممکن است شهر یافت نشده باشد.");
            }

            return ResultDto<bool>.Success("شهر با موفقیت بروزرسانی شد.", true);
        }
    }
}
