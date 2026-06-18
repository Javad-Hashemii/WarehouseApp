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
    public class InventoryService(IInventoryRepository inventoryRepository) : IInventoryService
    {
        public async Task<ResultDto<bool>> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct)
        {
            try
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
            catch
            {
                return ResultDto<bool>.Failure("خطا در ایجاد موجودی!", false);
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
        
    }
}
