using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class RequestService(IRequestRepository requestRepository,IProductService productService) : IRequestService
    {
        public async Task<ResultDto<bool>> CreateRequestAsync(CreateRequestDto createRequestDto, CancellationToken ct)
        {   

            if(createRequestDto == null||!createRequestDto.Items.Any())
            {
                return ResultDto<bool>.Failure("درخواست خالی است!");
            }
            var productsIds= createRequestDto.Items.Select(i => i.ProductId).ToList();
            var productsCheckResult = await productService.AreProductsExist(productsIds, ct);

            if(!productsCheckResult.IsSuccess)
            {
                return ResultDto<bool>.Failure("برخی از محصولات یافت نشدند.");
            }
            
            var result = await requestRepository.CreateRequestAsync(createRequestDto, ct);
            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ایجاد درخواست");
            }

            return ResultDto<bool>.Success("درخواست با موفقیت ایجاد شد", true);

        }

        public Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<GetRequestDto?> GetByIdWithDetailsAsync(int id,CancellationToken ct)
        {
            return await requestRepository.GetByIdWithDetailsAsync(id, ct);
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByStatusAsync(status, ct);
        }
        

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByWarehouseAsync(warehouseId, ct);
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByWorkshopAsync(workshopId, ct);
        }

        public async Task<ResultDto<bool>> UpdateRequestAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct)
        {
            var result = await requestRepository.UpdateRequestAsync(updateStatusDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("درخواست با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی درخواست", false);
            }
        }

        public async Task<ResultDto<bool>> UpdateStatusAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct)
        {
            var result = await requestRepository.UpdateStatusAsync(updateStatusDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("وضعیت درخواست با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی وضعیت درخواست", false);
            }

        }
        public async Task<ResultDto<bool>> ModifyRequestItemsAsync(
            int requestId,
            IReadOnlyCollection<AddRequestItemDto> newItems,
            IReadOnlyCollection<UpdateRequestItemDto> updatedItems,
            IReadOnlyCollection<int> deletedItemIds,CancellationToken ct)
        {
            var updateIds = updatedItems.Select(x => x.Id).Distinct().ToList();
            var deleteIds = deletedItemIds.Distinct().ToList();

            if (updateIds.Intersect(deleteIds).Any())
            {
                return ResultDto<bool>.Failure("خطا: نمیتوان هم یک آیتم را آپدیت و همان آیتم را حذف کرد");
            }

            var affectedIds = updateIds.Concat(deleteIds).Distinct().ToList();

            var existingItems = await requestRepository.GetRequestItemsByIdsAsync(requestId,affectedIds,ct);

            var itemsById = existingItems.ToDictionary(x => x.Id);

            if (affectedIds.Any(id => !itemsById.ContainsKey(id)))
            {
                return ResultDto<bool>.Failure("بعضی آیتم های انتخاب شده در این درخواست وجود ندارند");
            }

            foreach (var updateItem in updatedItems)
            {
                var entity = itemsById[updateItem.Id];

                entity.ProductId = updateItem.ProductId;
                entity.QuantityRequested = updateItem.QuantityRequested;
            }

            var itemsToDelete = deleteIds
                .Select(id => itemsById[id])
                .ToList();

            requestRepository.RemoveRequestItems(itemsToDelete);

            if (newItems.Count > 0)
            {
                var entities = newItems.Select(x => new RequestItem
                {
                    RequestId = requestId,
                    ProductId = x.ProductId,
                    QuantityRequested = x.QuantityRequested
                });

                await requestRepository.AddRequestItemsAsync(entities, ct);
            }

            await requestRepository.SaveChangesAsync(ct);

            return ResultDto<bool>.Success("درخواست با موفقیت بروزرسانی شد", true);
        }

    }
}
    

