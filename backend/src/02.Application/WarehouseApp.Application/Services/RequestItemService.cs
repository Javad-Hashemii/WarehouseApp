using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace WarehouseApp.Application.Services
{
    public class RequestItemService(IRequestItemRepository requestItemRepository) : IRequestItemService
    {
        public async Task<ResultDto<bool>> AddRequestItemAsync(AddRequestItemDto addRequestItemsDto, CancellationToken ct)
        {
            var result = await requestItemRepository.AddRequestItemAsync(addRequestItemsDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("ایتم درخواست با موفقیت اضافه شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در ایجاد ایتم درخواست", false);
            }
            
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct)
        {
            return await requestItemRepository.GetAllRequestItemsAsync(ct);
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct)
        {
            return await requestItemRepository.GetAllRequestItemsByRequestIdAsync(requestId, ct);
        }

        public async Task<ResultDto<bool>> UpdateRequestItem(UpdateRequestItemDto updateRequestItemDto, CancellationToken ct)
        {
            if (updateRequestItemDto == null)
            {
                return ResultDto<bool>.Failure("درخواست تغییر ایتم خالی است!");
            }
            else
            {
                var isRequestItemExist = await requestItemRepository.IsRequestItemExist(updateRequestItemDto.Id, ct);
                if (!isRequestItemExist)
                {
                    return ResultDto<bool>.Failure($"ایتم درخواست با آیدی {updateRequestItemDto.Id} پیدا نشد");
                }
                else
                {
                    var result = await requestItemRepository.UpdateRequestItemAsync(updateRequestItemDto, ct);
                    if (!result)
                    {
                        return ResultDto<bool>.Failure("تغییر درخواست موفقیت آمیز نبود!!");
                    }
                    else
                    {
                        return ResultDto<bool>.Success("آیتم درخواست با موفقیت تغییر کرد.");
                    }
                }
            }
        }
    }
}

