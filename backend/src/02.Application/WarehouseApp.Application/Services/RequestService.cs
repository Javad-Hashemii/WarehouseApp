using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Contracts.UnitOfWork;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class RequestService(IRequestRepository requestRepository,IWorkshopRepository workshopRepository,IWarehouseRepository warehouseRepository,IProductService productService,IRequestHistoryService requestHistoryService,ICurrentUserService currentUserService,IUnitOfWork unitOfWork) : IRequestService
    {
        public async Task<ResultDto<bool>> CreateRequestAsync(CreateRequestDto createRequestDto, CancellationToken ct)
        {
            if (createRequestDto == null || createRequestDto.Items == null || !createRequestDto.Items.Any())
            {
                return ResultDto<bool>.Failure("درخواست خالی است!");
            }

            if (createRequestDto.Items.Any(i => i.QuantityRequested <= 0))
            {
                return ResultDto<bool>.Failure("مقدار درخواست باید بیشتر از صفر باشد");
            }

            if (!currentUserService.WorkshopId.HasValue)
            {
                return ResultDto<bool>.Failure("کارگاه کاربر مشخص نشده است");
            }

            var workshopId = currentUserService.WorkshopId.Value;

            var isWorkshopExist = await workshopRepository.IsWorkshopExistByIdAsync(workshopId, ct);
            if (!isWorkshopExist)
            {
                return ResultDto<bool>.Failure("کارگاه یافت نشد");
            }

            var warehouseId = await warehouseRepository.GetWarehouseIdByWorkshopId(workshopId, ct);
            if (!warehouseId.HasValue)
            {
                return ResultDto<bool>.Failure("انبار مرتبط با کارگاه یافت نشد");
            }

            var isRequestNumberExist = await requestRepository.IsRequestNumberExistAsync(createRequestDto.RequestNumber, ct);
            if (isRequestNumberExist)
            {
                return ResultDto<bool>.Failure("شماره درخواست تکراری است");
            }

            var productIds = createRequestDto.Items.Select(i => i.ProductId).Distinct().ToList();
            var productsCheckResult = await productService.AreProductsExist(productIds, ct);

            if (!productsCheckResult.IsSuccess)
            {
                return ResultDto<bool>.Failure("برخی از محصولات یافت نشدند");
            }

            createRequestDto.RequestedByUserId = currentUserService.UserId;
            createRequestDto.WorkshopId = workshopId;
            createRequestDto.WarehouseId = warehouseId.Value;

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

        public async Task<ResultDto<GetRequestDto?>> GetByIdWithDetailsAsync(int id,CancellationToken ct)
        {
            if(id<=0)
            {
                return ResultDto<GetRequestDto?>.Failure("شناسه درخواست نامعتبر است");
            }

            var isRequestExist = await requestRepository.IsRequestIdExistAsync(id, ct);

            if(!isRequestExist)
            {
                return ResultDto<GetRequestDto?>.Failure("درخواست یافت نشد");
            }

            var result = await requestRepository.GetByIdWithDetailsAsync(id, ct);
            return ResultDto<GetRequestDto?>.Success("درخواست با موفقیت دریافت شد", result);
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

        public async Task<ResultDto<bool>> ModifyRequestItemsAsync(int requestId,IReadOnlyCollection<AddRequestItemDto> newItems,IReadOnlyCollection<UpdateRequestItemDto> updatedItems,IReadOnlyCollection<int> deletedItemIds,CancellationToken ct)
        {
            var request = await requestRepository.GetByIdAsync(requestId, ct);

            if (request is null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            if (!CanEditRequest(request))
            {
                return ResultDto<bool>.Failure(
                    "شما اجازه ویرایش این درخواست را ندارید");
            }

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
                    QuantityRequested = x.QuantityRequested,
                    QuantityIssued = 0,
                    QuantityReceived = 0,
                });

                await requestRepository.AddRequestItemsAsync(entities, ct);
            }

            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = request.Status,
                ToStatus = request.Status,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = DateTime.UtcNow,
                Notes = "بروزرسانی درخواست"
            };


            requestHistoryService.CreateRequestHistoryAsync(history);


            var result = await unitOfWork.SaveChangesAsync(ct);
            await requestRepository.SaveChangesAsync(ct);

            return ResultDto<bool>.Success("درخواست با موفقیت بروزرسانی شد", true);
        }

        //public async Task<ResultDto<bool>> ApproveWorkshopRequestAsync(int requestId,int userId,CancellationToken ct)
        //{
        //    var request = await requestRepository
        //        .GetByIdAsync(requestId, ct);

        //    if (request == null || request.Status != RequestStatus.PendingWorkshop)
        //    {
        //        return ResultDto<bool>.Failure("درخواست در وضعیت مناسبی برای تایید نیست");
        //    }

        //    var oldStatus = request.Status;

        //    request.Status = RequestStatus.PendingWarehouse;
        //    request.UpdatedAt = DateTime.UtcNow;
        //    request.UpdatedByUserId = userId;


        //    var history = new CreateRequestHistoryDto
        //    {
        //        RequestId = request.Id,
        //        FromStatus = oldStatus,
        //        ToStatus = request.Status,
        //        ChangedByUserId = userId,
        //        TimeChanged = DateTime.UtcNow,
        //        Notes = "تایید درخواست توسط کارگاه"
        //    };

        //    requestHistoryService.CreateRequestHistoryAsync(history);//1.This method already calls savechanges 2.the repository for it is not async for now check it


        //    var result = await unitOfWork.SaveChangesAsync(ct); //should we remove save changes from (1)?

        //    if (result <= 0)
        //    {
        //        return ResultDto<bool>.Failure("خطا در تایید درخواست");
        //    }

        //    return ResultDto<bool>.Success("درخواست با موفقیت توسط کارگاه تایید شد");
        //}

        public async Task<ResultDto<bool>> ApproveRequestAsync(int requestId,CancellationToken ct)
        {
            var request = await requestRepository
                .GetByIdAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }


            var newStatus = currentUserService.Role switch
            {
                Roles.WorkshopManager
                    when request.Status == RequestStatus.PendingWorkshop
                        => RequestStatus.PendingWarehouse,


                Roles.WarehouseManager
                    when request.Status == RequestStatus.PendingWarehouse
                        => RequestStatus.PendingCentralOffice,


                Roles.CentralManager
                    when request.Status == RequestStatus.PendingCentralOffice
                        => RequestStatus.PendingProcurement,


                _ => (RequestStatus?)null
            };


            if (newStatus == null)
            {
                return ResultDto<bool>.Failure("شما اجازه تایید این درخواست را ندارید");
            }


            var oldStatus = request.Status;


            request.Status = newStatus.Value;
            request.UpdatedAt = DateTime.UtcNow;
            request.UpdatedByUserId = currentUserService.UserId;


            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = oldStatus,
                ToStatus = newStatus.Value,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = DateTime.UtcNow,
                Notes = "تایید درخواست"
            };


            requestHistoryService.CreateRequestHistoryAsync(history);


            var result = await unitOfWork.SaveChangesAsync(ct);


            if (result<0)
            {
                return ResultDto<bool>.Failure("خطا در تایید درخواست");
            }


            return ResultDto<bool>.Success("درخواست با موفقیت تایید شد");
        }

        public async Task<ResultDto<List<GetAllRequestsDto>>>GetRequestsForCurrentUserAsync(CancellationToken ct)
        {
            var role = currentUserService.Role;

            if (role == Roles.Admin)
            {
                var allRequests = await requestRepository.GetAllSummariesAsync(ct);

                return ResultDto<List<GetAllRequestsDto>>.Success("درخواست‌ها دریافت شدند", allRequests);
            }


            var status = role switch
            {
                Roles.WorkshopEmployee => RequestStatus.PendingWorkshop,

                Roles.WorkshopManager => RequestStatus.PendingWorkshop,

                Roles.WarehouseManager => RequestStatus.PendingWarehouse,

                Roles.CentralManager => RequestStatus.PendingCentralOffice,

                _ => (RequestStatus?)null
            };


            if (status is null)
            {
                return ResultDto<List<GetAllRequestsDto>>.Failure("دسترسی ندارید");
            }


            var requests = await requestRepository.GetSummariesByStatusAsync(status.Value, ct);


            return ResultDto<List<GetAllRequestsDto>>.Success("درخواست‌ها دریافت شدند", requests);
        }

        public async Task<ResultDto<bool>> RejectRequestAsync(int requestId, CancellationToken ct)
        {
            var request = await requestRepository
                          .GetByIdAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }


            var newStatus = currentUserService.Role switch
            {
                Roles.WorkshopManager
                    when request.Status == RequestStatus.PendingWorkshop
                        => RequestStatus.RejectByWorkshopManager,


                Roles.CentralManager
                    when request.Status == RequestStatus.PendingCentralOffice
                        => RequestStatus.RejectByCentralOffice,


                _ => (RequestStatus?)null
            };


            if (newStatus == null)
            {
                return ResultDto<bool>.Failure("شما اجازه رد این درخواست را ندارید");
            }


            var oldStatus = request.Status;


            request.Status = newStatus.Value;
            request.UpdatedAt = DateTime.UtcNow;
            request.UpdatedByUserId = currentUserService.UserId;


            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = oldStatus,
                ToStatus = newStatus.Value,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = DateTime.UtcNow,
                Notes = "رد درخواست"
            };


            requestHistoryService.CreateRequestHistoryAsync(history);


            var result = await unitOfWork.SaveChangesAsync(ct);


            if (result < 0)
            {
                return ResultDto<bool>.Failure("خطا در رد درخواست");
            }


            return ResultDto<bool>.Success("درخواست با موفقیت رد شد");
        }

        private bool CanEditRequest(Request request)
        {
            var role = currentUserService.Role;

            if (role == Roles.Admin)
            {
                return true;
            }

            if (request.Status == RequestStatus.PendingWorkshop)
            {
                var hasWorkshopRole =
                    role == Roles.WorkshopEmployee ||
                    role == Roles.WorkshopManager;

                return hasWorkshopRole
                       && currentUserService.WorkshopId.HasValue
                       && currentUserService.WorkshopId.Value == request.WorkshopId;
            }

            if (request.Status == RequestStatus.PendingCentralOffice)
            {
                return role == Roles.CentralManager;
            }

            return false;
        }
        
    }
}
    

