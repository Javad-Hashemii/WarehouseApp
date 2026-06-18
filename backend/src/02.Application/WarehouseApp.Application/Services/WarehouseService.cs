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
                return ResultDto<bool>.Failure("کارگاه از قبل وجود دارد");
            }

            else
            {
                var result = await warehouseRepository.CreateWarehouseAsync(createWarehouseDto.Name, createWarehouseDto.CityId, ct);
                if (result)
                {
                    return ResultDto<bool>.Success("کارگاه با موفقیت ایجاد شد", true);

                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در ایجاد کارگاه");
                }
            }
        }

        public Task<bool> DeleteWarehouseAsync(int warehouseId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<GetWarehouseDetailDto> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct)
        {
            return await GetWarehouseDetailAsync(warehouseId, ct);
        }

        public async Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct)
        {
            return await warehouseRepository.GetWarehousesAsync(ct);
        }

        public async Task<ResultDto<bool>> UpdateWarehouseAsync(UpdateWarehouseDto updateWarehouseDto , CancellationToken ct)
        {
            var result = await warehouseRepository.UpdateWarehouseAsync(updateWarehouseDto.WarehouseId, updateWarehouseDto.Name, updateWarehouseDto.CityId, ct); // وای خداا
            if (result)
            {
                return ResultDto<bool>.Success("کارگاه با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی کارگاه");
            }

        }
    }
}


