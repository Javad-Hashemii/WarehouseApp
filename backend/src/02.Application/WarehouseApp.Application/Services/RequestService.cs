using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;
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
    public class RequestService(IRequestRepository requestRepository, IInventoryRepository inventoryRepository, IWorkshopRepository workshopRepository,
        IWarehouseRepository warehouseRepository, IProductService productService, IRequestHistoryService requestHistoryService,
        ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository,IPurchaseRequestRepository purchaseRequestRepository,ILogger<RequestService> logger) : IRequestService //inconsistency in design
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
                logger.LogWarning("User {UserId} attempted to create a request without an assigned workshop",
                    currentUserService.UserId);
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
                logger.LogWarning("No warehouse found for workshop {WorkshopId} while user {UserId} was creating a request",
                    workshopId,
                    currentUserService.UserId);
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

            logger.LogInformation("Request {RequestNumber} created by user {UserId} for Workshop {WorkshopId} and Warehouse {WarehouseId}",
                createRequestDto.RequestNumber,
                createRequestDto.RequestedByUserId,
                createRequestDto.WorkshopId,
                createRequestDto.WarehouseId);
            return ResultDto<bool>.Success("درخواست با موفقیت ایجاد شد", true);
        }

        public Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<GetRequestDto?>> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return ResultDto<GetRequestDto?>.Failure("شناسه درخواست نامعتبر است");
            }

            var isRequestExist = await requestRepository.IsRequestIdExistAsync(id, ct);

            if (!isRequestExist)
            {
                return ResultDto<GetRequestDto?>.Failure("درخواست یافت نشد");
            }

            var result = await requestRepository.GetByIdWithDetailsAsync(id, ct);
            return ResultDto<GetRequestDto?>.Success("درخواست با موفقیت دریافت شد", result);
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status, CancellationToken ct)
        {
            return await requestRepository.GetSummariesByStatusesAsync(new[] { status }, ct);
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId, CancellationToken ct)
        {
            return await requestRepository.GetSummariesByWarehouseAsync(warehouseId, ct);
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId, CancellationToken ct)
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

        public async Task<ResultDto<bool>> ModifyRequestItemsAsync(int requestId, IReadOnlyCollection<AddRequestItemDto> newItems,
            IReadOnlyCollection<UpdateRequestItemDto> updatedItems, IReadOnlyCollection<int> deletedItemIds, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdAsync(requestId, ct);

            if (request is null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            if (!CanEditRequest(request))
            {
                logger.LogWarning("Unauthorized attempt to modify request {RequestId} by user {UserId} with role {Role} while request status was {RequestStatus}",
                    requestId,
                    currentUserService.UserId,
                    currentUserService.Role,
                    request.Status);
                return ResultDto<bool>.Failure("شما اجازه ویرایش این درخواست را ندارید");
            }

            var updateIds = updatedItems.Select(x => x.Id).Distinct().ToList();
            var deleteIds = deletedItemIds.Distinct().ToList();

            if (updateIds.Intersect(deleteIds).Any())
            {
                return ResultDto<bool>.Failure("خطا: نمیتوان هم یک آیتم را آپدیت و همان آیتم را حذف کرد");
            }

            var affectedIds = updateIds.Concat(deleteIds).Distinct().ToList();

            var existingItems = await requestRepository.GetRequestItemsByIdsAsync(requestId, affectedIds, ct);

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

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در بروزرسانی درخواست");
            }

            logger.LogInformation("Request {RequestId} modified by user {UserId}: {AddedCount} added, {UpdatedCount} updated, {DeletedCount} deleted",
                requestId,
                currentUserService.UserId,
                newItems.Count,
                updateIds.Count,
                deleteIds.Count);
            return ResultDto<bool>.Success("درخواست با موفقیت بروزرسانی شد", true);
        }

        public async Task<ResultDto<bool>> ApproveRequestAsync(int requestId, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdAsync(requestId, ct);

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
                logger.LogWarning("Unauthorized attempt by user {UserId} with role {Role} to approve request {RequestId} in status {RequestStatus}",
                    currentUserService.UserId,
                    currentUserService.Role,
                    requestId,
                    request.Status);
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


            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در تایید درخواست");
            }

            logger.LogInformation("Request {RequestId} approved by user {UserId}: status changed from {OldStatus} to {NewStatus}",
                requestId,
                currentUserService.UserId,
                oldStatus,
                newStatus);

            return ResultDto<bool>.Success("درخواست با موفقیت تایید شد");
        }

        public async Task<ResultDto<List<GetAllRequestsDto>>> GetRequestsForCurrentUserAsync(CancellationToken ct)
        {
            var role = currentUserService.Role;

            if (role == Roles.Admin)
            {
                var allRequests = await requestRepository.GetAllSummariesAsync(ct);

                return ResultDto<List<GetAllRequestsDto>>.Success("درخواست‌ها دریافت شدند", allRequests);
            }

            var statuses = role switch
            {
                Roles.WorkshopEmployee => new[] { RequestStatus.PendingWorkshop },

                Roles.WorkshopManager => new[]
                {
                RequestStatus.PendingWorkshop,
                RequestStatus.PendingWorkshopReceipt,
                RequestStatus.PendingPurchaseDecision
                },

                Roles.WarehouseManager => new[] { RequestStatus.PendingWarehouse },

                Roles.CentralManager => new[] { RequestStatus.PendingCentralOffice },

                _ => Array.Empty<RequestStatus>()
            };

            if (statuses.Length == 0)
            {
                return ResultDto<List<GetAllRequestsDto>>.Failure("دسترسی ندارید");
            }

            var requests = await requestRepository.GetSummariesByStatusesAsync(statuses, ct);

            return ResultDto<List<GetAllRequestsDto>>.Success("درخواست‌ها دریافت شدند", requests);
        }

        public async Task<ResultDto<bool>> RejectRequestAsync(int requestId, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdAsync(requestId, ct);

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
                logger.LogWarning("Unauthorized attempt by user {UserId} with the role {Role} to reject Request {RequestId} with status {RequestStatus}",
                    currentUserService.UserId,
                    currentUserService.Role,
                    requestId,
                    request.Status);
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


            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در رد درخواست");
            }


            logger.LogInformation("Request {RequestId} rejected by user {UserId}: status changed from {OldStatus} to {NewStatus}",
                requestId,
                currentUserService.UserId,
                oldStatus,
                newStatus.Value);

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

        public async Task<ResultDto<GetRequestDto>> GetRequestInformationForWarehouseAsync(int requestId, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdWithDetailsAsync(requestId, ct);
            if (request == null)
            {
                return ResultDto<GetRequestDto>.Failure("درخواست یافت نشد");
            }

            if (request.Status != RequestStatus.PendingWarehouse)
            {
                return ResultDto<GetRequestDto>.Failure("درخواست در وضعیت مناسبی برای بررسی توسط انبار نیست");
            }

            var requestItemIds = request.Items.Select(i => i.ProductId).ToList();

            var inventoryBalances = await inventoryRepository.GetInventoryBalanceForProductsAsync(requestItemIds, request.WarehouseStockLocationId, ct);
            var inventoryByProductId = inventoryBalances.ToDictionary(i => i.ProductId);



            foreach (var item in request.Items)
            {
                var quantityAvailable = inventoryByProductId.TryGetValue(item.ProductId, out var inventory)
                    ? inventory.Quantity
                    : 0;

                var remainingQuantity = item.QuantityRequested - item.QuantityIssued;

                item.QuantityAvailable = quantityAvailable;
                item.QuantityToIssue = Math.Min(remainingQuantity, quantityAvailable);
                item.QuantityShortage = remainingQuantity - item.QuantityToIssue;
            }

            return ResultDto<GetRequestDto>.Success("اطلاعات درخواست با موفقیت دریافت شد", request);

        }

        public async Task<ResultDto<bool>> ProcessWarehouseRequestAsync(int requestId, CancellationToken ct)
        {
            var requestDto = await requestRepository.GetByIdWithDetailsAsync(requestId, ct);

            if (requestDto == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            if (requestDto.Status != RequestStatus.PendingWarehouse)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت مناسبی برای پردازش توسط انبار نیست");
            }

            if (!requestDto.WarehouseStockLocationId.HasValue || !requestDto.WorkshopStockLocationId.HasValue)
            {
                logger.LogWarning("Can not procces request {RequestId} because warehouse or workshop location is missing", requestId);
                return ResultDto<bool>.Failure("محل موجودی انبار یا کارگاه یافت نشد");
            }

            var request = await requestRepository.GetByIdAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            var requestItemIds = requestDto.Items.Select(i => i.Id).ToList();
            var requestItems = await requestRepository.GetRequestItemsByIdsAsync(requestId, requestItemIds, ct);

            var productIds = requestItems.Select(i => i.ProductId).Distinct().ToList();

            var inventoryBalances = await inventoryRepository.GetInventoryBalancesForUpdateAsync(productIds, requestDto.WarehouseStockLocationId.Value, ct);

            var inventoryByProductId = inventoryBalances.ToDictionary(i => i.ProductId);
            var transactions = new List<InventoryTransaction>();
            var now = DateTime.UtcNow;

            foreach (var item in requestItems)
            {
                var remainingQuantity = item.QuantityRequested - item.QuantityIssued;

                if (remainingQuantity <= 0)
                {
                    continue;
                }

                if (!inventoryByProductId.TryGetValue(item.ProductId, out var inventory))
                {
                    continue;
                }

                var quantityToIssue = Math.Min(remainingQuantity, inventory.Quantity);

                if (quantityToIssue <= 0)
                {
                    continue;
                }

                inventory.Quantity -= quantityToIssue;
                inventory.LastUpdatedAt = now;
                inventory.UpdatedAt = now;
                inventory.UpdatedByUserId = currentUserService.UserId;

                item.QuantityIssued += quantityToIssue;
                item.UpdatedAt = now;
                item.UpdatedByUserId = currentUserService.UserId;

                transactions.Add(new InventoryTransaction
                {
                    ProductId = item.ProductId,
                    RequestId = requestId,
                    RequestItemId = item.Id,

                    SourceStockLocationId = requestDto.WarehouseStockLocationId.Value,
                    DestinationStockLocationId = requestDto.WorkshopStockLocationId.Value,

                    TransactionType = TransactionType.StockOut,
                    Status = InventoryTransactionStatus.InTransit,

                    Quantity = quantityToIssue,
                    QuantityReceived = null,

                    TransactionDate = now,
                    PerformedByUserId = currentUserService.UserId,
                    CreatedAt = now,
                    CreatedByUserId = currentUserService.UserId
                });
            }

            if (transactions.Count > 0)
            {
                await transactionRepository.AddTransactionsAsync(transactions, ct);
            }

            var oldStatus = request.Status;

            request.Status = RequestStatus.PendingWorkshopReceipt;
            request.UpdatedAt = now;
            request.UpdatedByUserId = currentUserService.UserId;

            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = oldStatus,
                ToStatus = request.Status,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = now,
                Notes = "ارسال اقلام موجود از انبار به کارگاه"
            };

            requestHistoryService.CreateRequestHistoryAsync(history);

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در پردازش درخواست");
            }

            var totalIssuedQuantity = transactions.Sum(x => x.Quantity);

            logger.LogInformation(
                "Request {RequestId} processed by warehouse user {UserId}: {TransactionCount} transactions created, {TotalIssuedQuantity} units issued, status changed from {OldStatus} to {NewStatus}",
                requestId,
                currentUserService.UserId,
                transactions.Count,
                totalIssuedQuantity,
                oldStatus,
                request.Status);

            return ResultDto<bool>.Success("اقلام موجود با موفقیت از انبار ارسال شدند", true);
        }
        public async Task<ResultDto<GetRequestDto>> GetRequestDetailForWorkshop(int requestId, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdWithDetailsAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<GetRequestDto>.Failure("درخواست یافت نشد");
            }

            if (request.Status != RequestStatus.PendingWorkshopReceipt)
            {
                return ResultDto<GetRequestDto>.Failure("عدم دسترسی به این درخواست");
            }

            return ResultDto<GetRequestDto>.Success("درخواست با موفقیت تایید شد", request);

        }

        public async Task<ResultDto<bool>> ConfirmWorkshopReceiptAsync(ConfirmWorkshopReceiptDto confirmWorkshopReceipt, CancellationToken ct)
        {
            if (confirmWorkshopReceipt == null || confirmWorkshopReceipt.Items == null || !confirmWorkshopReceipt.Items.Any())
            {
                return ResultDto<bool>.Failure("درخواست نمیتواند خالی باشد");
            }

            var requestDetails = await requestRepository.GetByIdWithDetailsAsync(confirmWorkshopReceipt.RequestId, ct);

            if (requestDetails == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            if (requestDetails.Status != RequestStatus.PendingWorkshopReceipt)
            {
                return ResultDto<bool>.Failure("عدم دسترسی به درخواست");
            }

            if (!currentUserService.WorkshopId.HasValue || currentUserService.WorkshopId.Value != requestDetails.WorkshopId)
            {
                logger.LogWarning("Unauthorized attempt to confirm Workshop Receipt by user {UserId} with role {Role} for request {RequestId} in status {RequestStatus}",
                    currentUserService.UserId,
                    currentUserService.Role,
                    requestDetails.Id,
                    requestDetails.Status);
                return ResultDto<bool>.Failure("عدم دسترسی به درخواست");
            }

            if (!requestDetails.WorkshopStockLocationId.HasValue)
            {
                logger.LogWarning(
                       "Cannot confirm receipt for request {RequestId} because workshop {WorkshopId} has no stock location",
                       requestDetails.Id,
                       requestDetails.WorkshopId);
                return ResultDto<bool>.Failure("محل موجودی کارگاه یافت نشد");
            }

            var requestItemIds = confirmWorkshopReceipt.Items
                .Select(i => i.RequestItemId)
                .ToList();

            var requestItems = await requestRepository.GetRequestItemsByIdsAsync(requestDetails.Id, requestItemIds, ct);

            if (requestItems.Count != requestDetails.Items.Count || requestItems.Count != confirmWorkshopReceipt.Items.Count)
            {
                return ResultDto<bool>.Failure("اطلاعات اقلام درخواست نامعتبر است");
            }

            var confirmedItemsById = confirmWorkshopReceipt.Items
                .ToDictionary(i => i.RequestItemId);

            foreach (var requestItem in requestItems)
            {
                if (!confirmedItemsById.TryGetValue(requestItem.Id, out var confirmItem))
                {
                    return ResultDto<bool>.Failure("اطلاعات دریافت یکی از اقلام یافت نشد");
                }

                if (confirmItem.QuantityReceived < 0 || confirmItem.QuantityReceived > requestItem.QuantityIssued)
                {
                    return ResultDto<bool>.Failure("مقدار دریافت شده نامعتبر است");
                }

                requestItem.QuantityReceived = confirmItem.QuantityReceived;
            }

            var productIds = requestItems
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var inventoryBalances = await inventoryRepository.GetInventoryBalancesForUpdateAsync(
                productIds,
                requestDetails.WorkshopStockLocationId.Value,
                ct);

            var inventoryByProductId = inventoryBalances.ToDictionary(i => i.ProductId);

            var transactions = await transactionRepository.GetTransactionsByRequestIdForUpdateAsync(requestDetails.Id, ct);
            var transactionsByRequestItemId = transactions
                .Where(t => t.RequestItemId.HasValue)
                .GroupBy(t => t.RequestItemId.Value)
                .ToDictionary(g => g.Key, g => g.ToList());
            var newInventoryBalances = new List<InventoryBalance>();
            var now = DateTime.UtcNow;

            foreach (var requestItem in requestItems)
            {
                var quantityReceived = requestItem.QuantityReceived;

                if (quantityReceived > 0)
                {
                    if (inventoryByProductId.TryGetValue(requestItem.ProductId, out var inventory))
                    {
                        inventory.Quantity += quantityReceived;
                        inventory.LastUpdatedAt = now;
                        inventory.UpdatedAt = now;
                        inventory.UpdatedByUserId = currentUserService.UserId;
                    }
                    else
                    {
                        var newInventory = new InventoryBalance
                        {
                            ProductId = requestItem.ProductId,
                            StockLocationId = requestDetails.WorkshopStockLocationId.Value,
                            Quantity = quantityReceived,
                            LastUpdatedAt = now,
                            CreatedAt = now,
                            CreatedByUserId = currentUserService.UserId,
                            IsDeleted = false
                        };

                        newInventoryBalances.Add(newInventory);
                        inventoryByProductId.Add(requestItem.ProductId, newInventory);
                    }
                }

                requestItem.UpdatedAt = now;
                requestItem.UpdatedByUserId = currentUserService.UserId;

                UpdateReceivedTransactions(requestItem,quantityReceived,transactionsByRequestItemId,now);
            }

            if (newInventoryBalances.Any())
            {
                await inventoryRepository.AddInventoryBalancesAsync(newInventoryBalances, ct);
            }

            var request = await requestRepository.GetByIdAsync(requestDetails.Id, ct);

            if (request == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            var oldStatus = request.Status;

            if (requestItems.Any(i => i.QuantityReceived < i.QuantityRequested))
            {
                request.Status = RequestStatus.PendingPurchaseDecision;
            }
            else
            {
                request.Status = RequestStatus.Fulfilled;
            }

            request.UpdatedAt = now;
            request.UpdatedByUserId = currentUserService.UserId;

            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = oldStatus,
                ToStatus = request.Status,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = now,
                Notes = "تایید اقلام دریافت شده توسط کارگاه"
            };

            requestHistoryService.CreateRequestHistoryAsync(history);

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در تایید اقلام دریافت شده");
            }

            var totalReceivedQuantity = requestItems.Sum(x => x.QuantityReceived);

            logger.LogInformation(
                "Workshop receipt confirmed for request {RequestId} by user {UserId}: {ItemCount} items with total received quantity {TotalReceivedQuantity}; status changed from {OldStatus} to {NewStatus}",
                request.Id,
                currentUserService.UserId,
                requestItems.Count,
                totalReceivedQuantity,
                oldStatus,
                request.Status);
            return ResultDto<bool>.Success("اقلام دریافت شده با موفقیت تایید شدند", true);
        }

        public async Task<ResultDto<GetRequestDto>> GetRequestForPurchaseDecisionAsync(int requestId, CancellationToken ct)
        {
            var request = await requestRepository.GetByIdWithDetailsAsync(requestId, ct);
            if (request == null)
            {
                return ResultDto<GetRequestDto>.Failure("درخواست یافت نشد");
            }
            if (request.Status != RequestStatus.PendingPurchaseDecision)
            {
                return ResultDto<GetRequestDto>.Failure("عدم دسترسی به این درخواست");
            }
            if(currentUserService.WorkshopId!= request.WorkshopId)
            {
                return ResultDto<GetRequestDto>.Failure("عدم دسترسی به این درخواست");
            }

            foreach (var item in request.Items)
            {
                item.QuantityShortage = item.QuantityRequested - item.QuantityReceived;
            }

            return ResultDto<GetRequestDto>.Success("درخواست با موفقیت دریافت شد", request);

        }

        public async Task<ResultDto<bool>> ConfirmPurchaseDecisionAsync(ConfirmPurchaseDecisionDto dto, CancellationToken ct)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                return ResultDto<bool>.Failure("اطلاعات درخواست خرید نامعتبر است");
            }

            var request = await requestRepository.GetByIdAsync(dto.RequestId, ct);

            if (request == null)
            {
                return ResultDto<bool>.Failure("درخواست یافت نشد");
            }

            if (request.Status != RequestStatus.PendingPurchaseDecision)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت مناسبی برای تصمیم خرید نیست");
            }

            if (!currentUserService.WorkshopId.HasValue ||
                currentUserService.WorkshopId.Value != request.WorkshopId)
            {
                logger.LogWarning(
                       "User {UserId} from workshop {UserWorkshopId} attempted to create a purchase request for request {RequestId} belonging to workshop {RequestWorkshopId}",
                       currentUserService.UserId,
                       currentUserService.WorkshopId,
                       request.Id,
                       request.WorkshopId);

                return ResultDto<bool>.Failure("عدم دسترسی به این درخواست");
            }

            var duplicateProductExists = dto.Items
                .GroupBy(i => i.ProductId)
                .Any(g => g.Count() > 1);

            if (duplicateProductExists)
            {
                return ResultDto<bool>.Failure("یک کالا بیش از یک بار در درخواست خرید ثبت شده است");
            }

            if (dto.Items.Any(i => i.QuantityToPurchase <= 0))
            {
                return ResultDto<bool>.Failure("مقدار خرید باید بیشتر از صفر باشد");
            }

            var requestItemIds = dto.Items
                .Where(i => i.RequestItemId.HasValue)
                .Select(i => i.RequestItemId!.Value)
                .ToList();

            var requestItems = await requestRepository.GetRequestItemsByIdsAsync(
                request.Id,
                requestItemIds,
                ct);

            if (requestItems.Count != requestItemIds.Count)
            {
                return ResultDto<bool>.Failure("برخی از اقلام انتخاب شده متعلق به این درخواست نیستند");
            }
            var requestItemsById = requestItems.ToDictionary(i => i.Id);

            foreach (var item in dto.Items.Where(i => i.RequestItemId.HasValue))
            {
                var requestItem = requestItemsById[item.RequestItemId!.Value];

                if (requestItem.ProductId != item.ProductId)
                {
                    return ResultDto<bool>.Failure("اطلاعات یکی از اقلام درخواست نامعتبر است");
                }
            }

            var newProductIds = dto.Items
                .Where(i => !i.RequestItemId.HasValue)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            if (newProductIds.Count > 0)
            {
                var productsExist = await productService.AreProductsExist(newProductIds, ct);

                if (!productsExist.IsSuccess)
                {
                    return ResultDto<bool>.Failure("برخی از کالاهای اضافه شده یافت نشدند");
                }
            }

            var purchaseRequest = new PurchaseRequest
            {
                PurchaseRequestNumber = $"{request.RequestNumber}-P",
                RequestId = request.Id,
                WorkshopId = request.WorkshopId,
                Status = PurchaseRequestStatus.PendingCentralOffice,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,

                Items = dto.Items.Select(i => new PurchaseRequestItem
                {
                    RequestItemId = i.RequestItemId,
                    ProductId = i.ProductId,
                    QuantityRequested = i.QuantityToPurchase,
                    QuantityApproved = 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId
                }).ToList()
            };

            await purchaseRequestRepository.AddAsync(purchaseRequest, ct);

            var oldStatus = request.Status;

            request.Status = RequestStatus.PendingPurchase;
            request.UpdatedAt = DateTime.UtcNow;
            request.UpdatedByUserId = currentUserService.UserId;

            var history = new CreateRequestHistoryDto
            {
                RequestId = request.Id,
                FromStatus = oldStatus,
                ToStatus = request.Status,
                ChangedByUserId = currentUserService.UserId,
                TimeChanged = DateTime.UtcNow,
                Notes = "درخواست خرید توسط مدیر کارگاه تایید و ارسال شد"
            };

            requestHistoryService.CreateRequestHistoryAsync(history);

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در ثبت درخواست خرید");
            }

            logger.LogInformation(
                "Purchase request {PurchaseRequestId} created from request {RequestId} by user {UserId} for workshop {WorkshopId} with {ItemCount} items; request status changed from {OldStatus} to {NewStatus}",
                purchaseRequest.Id,
                request.Id,
                currentUserService.UserId,
                request.WorkshopId,
                purchaseRequest.Items.Count,
                oldStatus,
                request.Status);

            return ResultDto<bool>.Success("درخواست خرید با موفقیت ثبت شد", true);
        }

        private void UpdateReceivedTransactions(RequestItem requestItem, decimal quantityReceived, Dictionary<int, List<InventoryTransaction>> transactionsByRequestItemId, DateTime now)
        {
            if (requestItem.QuantityIssued <= 0)
            {
                return;
            }

            if (!transactionsByRequestItemId.TryGetValue(requestItem.Id, out var transactions))
            {
                throw new InvalidOperationException("تراکنش ارسال یکی از اقلام یافت نشد");
            }

            var remainingQuantity = quantityReceived;

            foreach (var transaction in transactions)
            {
                if (remainingQuantity <= 0)
                {
                    break;
                }

                var pendingQuantity = transaction.Quantity - (transaction.QuantityReceived ?? 0);

                if (pendingQuantity <= 0)
                {
                    continue;
                }

                var receivedQuantity = Math.Min(pendingQuantity, remainingQuantity);

                transaction.QuantityReceived =
                    (transaction.QuantityReceived ?? 0) + receivedQuantity;

                if (transaction.QuantityReceived >= transaction.Quantity)
                {
                    transaction.Status = InventoryTransactionStatus.Received;
                    transaction.ReceivedAt = now;
                }

                transaction.UpdatedAt = now;
                transaction.UpdatedByUserId = currentUserService.UserId;

                remainingQuantity -= receivedQuantity;
            }
        }

        public async Task<ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>> GetRequestsForHistoryAsync(string? search,int? workshopId,int? warehouseId,int page,int pageSize,CancellationToken ct)
        {
            var role = currentUserService.Role;

            if (role == Roles.WorkshopEmployee ||
                role == Roles.WorkshopManager)
            {
                if (!currentUserService.WorkshopId.HasValue)
                {
                    return ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>
                        .Failure("کارگاه کاربر مشخص نشده است");
                }

                // Force access to the user's own workshop
                workshopId = currentUserService.WorkshopId.Value;

                // Ignore warehouse filter coming from the browser
                warehouseId = null;
            }
            else if (role == Roles.WarehouseManager)
            {
                if (!currentUserService.WorkshopId.HasValue)
                {
                    return ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>
                        .Failure("کارگاه کاربر مشخص نشده است");
                }

                // Get the warehouse related to the user's workshop
                var userWarehouseId = await warehouseRepository
                    .GetWarehouseIdByWorkshopId(
                        currentUserService.WorkshopId.Value,
                        ct);

                if (!userWarehouseId.HasValue)
                {
                    return ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>
                        .Failure("انبار مرتبط با کاربر یافت نشد");
                }

                // Force access to the user's own warehouse
                warehouseId = userWarehouseId.Value;

                // Ignore workshop filter coming from the browser
                workshopId = null;
            }
            else if (role != Roles.Admin &&
                     role != Roles.CentralManager)
            {
                return ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>
                    .Failure("شما اجازه مشاهده تاریخچه درخواست‌ها را ندارید");
            }

            var result = await requestRepository.GetRequestsForHistoryAsync(
                search,
                workshopId,
                warehouseId,
                page,
                pageSize,
                ct);

            return ResultDto<PaginatedResult<GetAllRequestsForHistoryDto>>.Success("تاریخچه درخواست‌ها با موفقیت دریافت شد", result);
        }

    }
}
    

