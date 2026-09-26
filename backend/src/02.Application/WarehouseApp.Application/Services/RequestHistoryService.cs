using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class RequestHistoryService(
    IRequestHistoryRepository requestHistoryRepository,IPurchaseRequestHistoryRepository purchaseRequestHistoryRepository,IRequestRepository requestRepository,IWarehouseRepository warehouseRepository,ICurrentUserService currentUserService) : IRequestHistoryService
    {
        public async void CreateRequestHistoryAsync(CreateRequestHistoryDto createRequestHistoryDto) //void???
        {


            var result =  requestHistoryRepository.CreateRequestHistory(createRequestHistoryDto);
            //if (result)
            //{
            //    return ResultDto<bool>.Success("Request history created successfully."); //should it return a message?
            //}
            //else
            //{
            //    return ResultDto<bool>.Failure("Failed to create request history."); //should it return a message?
            //}
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct)
        {
            return await requestHistoryRepository.GetAllRequestHistoriesAsync(ct);
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId,CancellationToken ct)
        {
            return await requestHistoryRepository.GetAllRequestHistoriesByRequestIdAsync(requestId, ct);
        }

        public async Task<ResultDto<List<GetRequestHistoryDetail>>> GetRequestHistoryDetailsAsync(int requestId,CancellationToken ct)
        {
            var requestHistories =await requestHistoryRepository.GetRequestHistoryDetailsAsync(requestId, ct);

            if (!requestHistories.Any())
            {
                return ResultDto<List<GetRequestHistoryDetail>>.Failure("تاریخچه‌ای برای این درخواست یافت نشد",requestHistories);
            }

            return ResultDto<List<GetRequestHistoryDetail>>.Success("تاریخچه درخواست با موفقیت دریافت شد",requestHistories);
        }

        public async Task<ResultDto<RequestHistoryDetailsDto>> GetDetailsAsync(int requestId,CancellationToken ct)
        {
            if (requestId <= 0)
            {
                return ResultDto<RequestHistoryDetailsDto>
                    .Failure("شناسه درخواست نامعتبر است");
            }

            var request = await requestRepository.GetByIdWithHistoryDetailsAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<RequestHistoryDetailsDto>
                    .Failure("درخواست یافت نشد");
            }

            var role = currentUserService.Role;

            if (role == Roles.WorkshopEmployee ||
                role == Roles.WorkshopManager)
            {
                if (!currentUserService.WorkshopId.HasValue ||
                    currentUserService.WorkshopId.Value != request.WorkshopId)
                {
                    return ResultDto<RequestHistoryDetailsDto>
                        .Failure("شما اجازه مشاهده تاریخچه این درخواست را ندارید");
                }
            }
            else if (role == Roles.WarehouseManager)
            {
                if (!currentUserService.WorkshopId.HasValue)
                {
                    return ResultDto<RequestHistoryDetailsDto>
                        .Failure("کارگاه کاربر مشخص نشده است");
                }

                var userWarehouseId = await warehouseRepository
                    .GetWarehouseIdByWorkshopId(
                        currentUserService.WorkshopId.Value,
                        ct);

                if (!userWarehouseId.HasValue ||
                    request.WarehouseId != userWarehouseId.Value)
                {
                    return ResultDto<RequestHistoryDetailsDto>
                        .Failure("شما اجازه مشاهده تاریخچه این درخواست را ندارید");
                }
            }
            else if (role != Roles.Admin &&
                     role != Roles.CentralManager)
            {
                return ResultDto<RequestHistoryDetailsDto>
                    .Failure("شما اجازه مشاهده تاریخچه این درخواست را ندارید");
            }

            var requestHistory = await requestHistoryRepository
                .GetRequestTimelineAsync(requestId, ct);

            var purchaseHistory = await purchaseRequestHistoryRepository
                .GetRequestTimelineAsync(requestId, ct);

            var timeline = requestHistory
                .Concat(purchaseHistory)
                .OrderBy(x => x.ChangedAt)
                .ToList();

            var result = new RequestHistoryDetailsDto
            {
                RequestId = request.Id,
                RequestNumber = request.RequestNumber,
                WorkshopName = request.Workshop.Name,
                WarehouseName = request.Warehouse?.Name,

                CurrentStatus =
                    request.Status == RequestStatus.PendingPurchase &&
                    purchaseHistory.Any()
                        ? purchaseHistory
                            .OrderByDescending(x => x.ChangedAt)
                            .First()
                            .ToStatus
                        : request.Status.ToString(),

                CreatedAt = request.CreatedAt,
                Timeline = timeline
            };

            return ResultDto<RequestHistoryDetailsDto>
                .Success("تاریخچه درخواست با موفقیت دریافت شد", result);
        }
    }
    
}
