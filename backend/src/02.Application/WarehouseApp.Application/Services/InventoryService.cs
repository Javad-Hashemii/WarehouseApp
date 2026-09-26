using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.City;
using WarehouseApp.Domain.Core.Dtos.InventoryBalance;

namespace WarehouseApp.Application.Services
{
    public class InventoryService(IInventoryRepository inventoryRepository,IWarehouseRepository warehouseRepository,IWorkshopRepository workshopRepository,ICurrentUserService currentUserService ) : IInventoryService
    {
        public async Task<ResultDto<bool>> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct)
        {

            bool isInventoryExist = await inventoryRepository.IsInventoryExistAsync(createInventoryDto.ProductId, createInventoryDto.WarehouseId, ct);
            if (isInventoryExist)
            {
                return ResultDto<bool>.Failure("این کالا در این انبار وجود دارد!");
            }
            var result = await inventoryRepository.CreateInventoryAsync(createInventoryDto, ct);
            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ایجاد موجودی!", false);
            }
            else
            {
                return ResultDto<bool>.Success("موجودی با موفقیت ایجاد شد!", true);
            }

        }

        public async Task<List<GetAllInventoryDto>> GetAllInventoriesAsync(CancellationToken ct)
        {
            return await inventoryRepository.GetAllInventoriesAsync(ct);
        }

        public async Task<ResultDto<bool>> UpdateInventoryAsync(UpdateInventoryDto updateInventoryBalanceDto, CancellationToken ct)
        {
            if (updateInventoryBalanceDto == null)
            {
                return ResultDto<bool>.Failure("اطلاعات ورودی نامعتبر است.");
            }

            else
            {
                var result = await inventoryRepository.UpdateInventoryAsync(updateInventoryBalanceDto, ct);
                if (result)
                {
                    return ResultDto<bool>.Success("موجودی با موفقیت بروزرسانی شد.", true);
                }
                else
                {
                    return ResultDto<bool>.Failure("خطا در بروزرسانی موجودی.", false);
                }
            }
        }

        public async Task<GetInventoryLocationsDto> GetInventoryLocationsAsync(CancellationToken ct)
        {
            var warehouses =
                await warehouseRepository.GetWarehouseInventoryLocationsAsync(ct);

            var workshops =
                await workshopRepository.GetWorkshopInventoryLocationsAsync(ct);

            return new GetInventoryLocationsDto
            {
                Warehouses = warehouses,
                Workshops = workshops
            };
        }

        public async Task<InventoryDetailsDto?> GetInventoryDetailsAsync(int stockLocationId,CancellationToken ct)
        {
            return await inventoryRepository
                .GetInventoryDetailsAsync(stockLocationId, ct);
        }

        public async Task<ResultDto<ManagerInventoryLocationsDto>> GetManagerInventoryLocationsAsync(CancellationToken ct)
        {
            if (!currentUserService.WorkshopId.HasValue)
                return ResultDto<ManagerInventoryLocationsDto>.Failure("کارگاه کاربر مشخص نشده است");

            var result = await inventoryRepository
                .GetManagerInventoryLocationsAsync(
                    currentUserService.WorkshopId.Value,
                    ct);

            if (result == null)
                return ResultDto<ManagerInventoryLocationsDto>.Failure("اطلاعات موجودی کارگاه یافت نشد");

            return ResultDto<ManagerInventoryLocationsDto>.Success("اطلاعات موجودی با موفقیت دریافت شد", result);
        }

    }
}
