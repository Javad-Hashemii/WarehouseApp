using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Warehouse;

namespace WarehouseApp.Application.Services
{
    public class WarehouseService(IWarehouseRepository warehouseRepository) : IWarehouseService
    {
        public async Task<ResultDto<bool>> CreateWarehouseAsync(CreateWarehouseDto createWarehouseDto, CancellationToken ct)
        {
            bool isWarehouseExist = await warehouseRepository.IsWarehouseExistAsync(createWarehouseDto.Name, ct);
            if (isWarehouseExist)
            {
                return ResultDto<bool>.Failure("انبار از قبل وجود دارد");
            }

            else
            {
                var result = await warehouseRepository.CreateWarehouseAsync(createWarehouseDto.Name, createWarehouseDto.CityId, ct);
                if (result)
                {
                    return ResultDto<bool>.Success("انبار با موفقیت ایجاد شد", true);

                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در ایجاد انبار");
                }
            }
        }

        public async Task<ResultDto<bool>> DeleteWarehouseAsync(int warehouseId,int deletedByUserId, CancellationToken ct)
        {
            if (warehouseId <= 0)
            {
                return ResultDto<bool>.Failure("انبار غیر معتبر");
            }
            if (deletedByUserId <= 0)
            {
                return ResultDto<bool>.Failure("انبار غیر معتبر");
            }

            bool isWarehouseExist = await warehouseRepository.IsWarehouseExistById(warehouseId,ct);
            if (!isWarehouseExist)
            {
                return ResultDto<bool>.Failure("انبار وجود نداره");
            }

            var result = await warehouseRepository.DeleteWarehouseAsync(warehouseId, deletedByUserId, ct);

            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در حذف انبار");
            }

            return ResultDto<bool>.Success("انبار با موفقیت حذف شد");
        }

        public async Task<GetWarehouseDetailDto> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct)
        {
            return await warehouseRepository.GetWarehouseDetailAsync(warehouseId, ct);
        }

        public async Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct)
        {
            return await warehouseRepository.GetWarehousesAsync(ct);
        }

        public async Task<ResultDto<bool>> UpdateWarehouseAsync(UpdateWarehouseDto updateWarehouseDto , CancellationToken ct)
        {
            var result = await warehouseRepository.UpdateWarehouseAsync(updateWarehouseDto.Id, updateWarehouseDto.Name, updateWarehouseDto.CityId, ct); // وای خداا
            if (result)
            {
                return ResultDto<bool>.Success("انبار با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی انبار");
            }

        }
    }
}


