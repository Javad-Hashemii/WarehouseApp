using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Workshop;

namespace WarehouseApp.Application.Services
{
    public class WorkShopService(IWorkshopRepository workshopRepository) : IWorkshopService
    {
        public async Task<ResultDto<bool>> CreateWorkshopAsync(CreateWorkshopDto createWorkshopDto, CancellationToken ct)
        {
            bool isWorkshopExist = await workshopRepository.IsWorkshopExistAsync(createWorkshopDto.Name, ct);
            if (isWorkshopExist)
            {
                return ResultDto<bool>.Failure("کارگاه با این نام قبلاً ایجاد شده است");
            }

            var result = await workshopRepository.CreateWorkshopAsync(createWorkshopDto.Name, createWorkshopDto.CityId, ct);
            if (result)
            {
                return ResultDto<bool>.Success("کارگاه با موفقیت ایجاد شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در ایجاد کارگاه");
            }
        }

        public async Task<List<GetAllWorkshopsDto>> GetWorkshopsAsync(CancellationToken ct)
        {
            return await workshopRepository.GetWorkshopsAsync(ct);
        }

        public async Task<ResultDto<bool>> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct)
        {
            var result = await workshopRepository.UpdateWorkshopAsync(updateWorkshopDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("کارگاه با موفقیت به‌روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به‌روزرسانی کارگاه");
            }
        }
    }
}
