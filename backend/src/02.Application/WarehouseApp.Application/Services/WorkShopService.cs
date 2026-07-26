using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Workshop;

namespace WarehouseApp.Application.Services
{
    public class WorkshopService(IWorkshopRepository workshopRepository) : IWorkshopService
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

        public async Task<ResultDto<GetWorkshopDetailDto?>> GetWorkshopDetailAsync(int workshopId, CancellationToken ct)
        {
            if (workshopId <= 0)
            {
                return ResultDto<GetWorkshopDetailDto?>.Failure("شناسه کارگاه نامعتبر است");
            }

            var workshop = await workshopRepository.GetWorkshopDetailAsync(workshopId, ct);
            if (workshop == null)
            {
                return ResultDto<GetWorkshopDetailDto?>.Failure("کارگاه یافت نشد");
            }

            return ResultDto<GetWorkshopDetailDto?>.Success("کارگاه با موفقیت دریافت شد", workshop);
        }

        public async Task<List<GetAllWorkshopsDto>> GetWorkshopsAsync(CancellationToken ct)
        {
            return await workshopRepository.GetWorkshopsAsync(ct);
        }

        public async Task<ResultDto<bool>> IsWorkshopExistByIdAsync(int workshopId, CancellationToken ct)
        {
            if (workshopId <= 0)
            {
                return ResultDto<bool>.Failure("شناسه کارگاه نامعتبر است");
            }
            var isExist = await workshopRepository.IsWorkshopExistByIdAsync(workshopId, ct);
            if (isExist)
            {
                return ResultDto<bool>.Success("کارگاه با این شناسه وجود دارد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("کارگاه با این شناسه وجود ندارد");
            }
        }

        public async Task<ResultDto<bool>> SoftDeleteWorkshopAsync(int workshopId, int deletedByUserId, CancellationToken ct)
        {
            if (workshopId <= 0)
            {
                return ResultDto<bool>.Failure("شناسه کارگاه نامعتبر است");
            }

            if (deletedByUserId <= 0)
            {
                return ResultDto<bool>.Failure("شناسه کاربر حذف کننده نامعتبر است");
            }

            var exists = await workshopRepository.IsWorkshopExistByIdAsync(workshopId, ct);
            if (!exists)
            {
                return ResultDto<bool>.Failure("کارگاه با این شناسه وجود ندارد");
            }

            var result = await workshopRepository.SoftDeleteWorkshopAsync(workshopId, deletedByUserId, ct);
            if (result)
            {
                return ResultDto<bool>.Success("کارگاه با موفقیت حذف شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در حذف کارگاه");
            }
        }


        public async Task<ResultDto<bool>> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct)
        {
            if (updateWorkshopDto.Id <= 0)
            {
                return ResultDto<bool>.Failure("شناسه کارگاه نامعتبر است");
            }

            bool isWorkshopExist = await workshopRepository.IsWorkshopExistByIdAsync(updateWorkshopDto.Id, ct);
            if (!isWorkshopExist)
            {
                return ResultDto<bool>.Failure("کارگاه با این شناسه وجود ندارد");
            }
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
