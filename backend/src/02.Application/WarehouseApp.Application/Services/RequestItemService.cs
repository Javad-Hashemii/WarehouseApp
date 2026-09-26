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

        public async Task<ResultDto<bool>> ApproveRequestItemsAsync(int requestId,List<ApproveRequestItemDto> items,CancellationToken ct)
        {
            if (requestId <= 0)
            {
                return ResultDto<bool>.Failure("آیدی درخواست نامعتبر است");
            }

            if (items == null || items.Count == 0)
            {
                return ResultDto<bool>.Failure("لیست آیتم های درخواست برای تایید خالی است");
            }

            var ids= items.Select(i => i.RequestItemId).Distinct().ToList();
            var requestItems = await requestItemRepository.GetRequestItemsByIdsAsync(requestId,ids, ct);


            var requestItemsById = requestItems
                .ToDictionary(i => i.Id);

            if (requestItemsById.Count != ids.Count)
            {
                return ResultDto<bool>.Failure("بعضی از آیتم های درخواست پیدا نشدند");
            }


            foreach (var approvedItem in items)
            {
                var requestItem = requestItemsById[approvedItem.RequestItemId];

                if (approvedItem.QuantityApproved < 0)
                {
                    return ResultDto<bool>.Failure("مقدار تایید شده نمی‌تواند منفی باشد");
                }

                if (approvedItem.QuantityApproved > requestItem.QuantityRequested)
                {
                    return ResultDto<bool>.Failure("مقدار تایید شده نمی‌تواند بیشتر از مقدار درخواستی باشد");
                }

                requestItem.QuantityApproved = approvedItem.QuantityApproved;
            }


            var result = await requestItemRepository.SaveChangesAsync(ct);

            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ذخیره تایید درخواست");
            }


            return ResultDto<bool>.Success("آیتم های درخواست با موفقیت تایید شدند");
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

