using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Contracts.UnitOfWork;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.PurchaseRequest;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class PurchaseRequestService(IPurchaseRequestRepository purchaseRequestRepository,ICurrentUserService currentUserService,IInventoryRepository inventoryRepository,IUnitOfWork unitOfWork):IPurchaseRequestService
    {
        public async Task<ResultDto<List<GetAllPurchaseRequestsDto>>> GetPurchaseRequestsForCurrentUserAsync(CancellationToken ct)
        {
            var role = currentUserService.Role;

            if (role == Roles.Admin)
            {
                var allStatuses = Enum.GetValues<PurchaseRequestStatus>();

                var allRequests = await purchaseRequestRepository.GetSummariesByStatusesAsync(allStatuses, ct);

                return ResultDto<List<GetAllPurchaseRequestsDto>>.Success("درخواست‌های خرید دریافت شدند", allRequests);
            }

            var statuses = role switch
            {
                Roles.CentralManager => new[]
                {
                PurchaseRequestStatus.PendingCentralOffice
                },

                Roles.ProcurementOfficer => new[]
                {
                PurchaseRequestStatus.PendingProcurement
                },

                Roles.WarehouseManager => new[]
                {
                PurchaseRequestStatus.PendingWarehouseReceipt
                },

                _ => Array.Empty<PurchaseRequestStatus>()
            };

            if (statuses.Length == 0)
            {
                return ResultDto<List<GetAllPurchaseRequestsDto>>.Failure("دسترسی ندارید");
            }

            var requests = await purchaseRequestRepository.GetSummariesByStatusesAsync(statuses, ct);

            return ResultDto<List<GetAllPurchaseRequestsDto>>.Success("درخواست‌های خرید دریافت شدند", requests);
        }
        public async Task<ResultDto<GetPurchaseRequestDto>>GetPurchaseRequestDetailForCentralManagerAsync(int requestId, CancellationToken ct)
        {
            if (requestId <= 0)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("شناسه درخواست نامعتبر است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequestWithDetail(requestId, ct);

            if(purchaseRequest == null)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("درخواست پیدا نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingCentralOffice)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("عدم دسترسی به این درخواست");
            }



            return ResultDto<GetPurchaseRequestDto>.Success("درخواست با موفقیت دریافت شد", purchaseRequest);
        }
        public async Task<ResultDto<bool>> ApprovePurchaseRequestAsync(ApprovePurchaseRequestDto approvePurchaseRequestDto, CancellationToken ct)
        {
            if (approvePurchaseRequestDto == null || !approvePurchaseRequestDto.Items.Any())
            {
                return ResultDto<bool>.Failure("درخواست نامعتبر است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequest(approvePurchaseRequestDto.PurchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<bool>.Failure("درخواست مورد نظر یافت نشد");
            }
            if (purchaseRequest.Status != PurchaseRequestStatus.PendingCentralOffice)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت مناسبی نیست");
            }
            var itemIds = approvePurchaseRequestDto.Items
                .Select(i => i.PurchaseRequestItemId)
                .ToList();

            if (itemIds.Distinct().Count() != itemIds.Count)
            {
                return ResultDto<bool>.Failure("اقلام تکراری در درخواست وجود دارد");
            }

            if (purchaseRequest.Items.Count != itemIds.Count ||
                purchaseRequest.Items.Any(i => !itemIds.Contains(i.Id)))
            {
                return ResultDto<bool>.Failure("اطلاعات اقلام درخواست نامعتبر است");
            }
            var itemsById = approvePurchaseRequestDto.Items.ToDictionary(pr => pr.PurchaseRequestItemId);

            foreach (var item in purchaseRequest.Items)
            {
                var updatedItem = itemsById[item.Id];

                if (updatedItem.QuantityApproved < 0 || updatedItem.QuantityApproved > item.QuantityRequested)
                {
                    return ResultDto<bool>.Failure("مقدار تایید شده نامعتبر است");
                }
                item.QuantityApproved = updatedItem.QuantityApproved;

            }

            var fromStatus = purchaseRequest.Status;

            if (purchaseRequest.Items.All(i => i.QuantityApproved == 0))
            {
                purchaseRequest.Status = PurchaseRequestStatus.RejectedByCentralOffice;
            }
            else
            {
                purchaseRequest.Status = PurchaseRequestStatus.PendingProcurement;
            }

            var purchaseRequestHistory = new PurchaseRequestHistory
            {
                PurchaseRequestId = purchaseRequest.Id,
                FromStatus = fromStatus,
                ToStatus = purchaseRequest.Status,
                ChangedAt = DateTime.UtcNow,
                ChangedByUserId = currentUserService.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                Notes = "خرید توسط دفتر مرکزی تایید شد",
            };
            purchaseRequest.History.Add(purchaseRequestHistory);
            var result = await unitOfWork.SaveChangesAsync(ct)>0;

            if (!result)
            {
                return ResultDto<bool>.Failure("خطا در ذخیره تغییرات");
            }

            return ResultDto<bool>.Success("درخواست با موفقیت ذخیره شد");
        }
        public async Task<ResultDto<bool>> DenyPurchaseRequestAsync(int purchaseRequestId, CancellationToken ct)
        {
            if (purchaseRequestId <= 0)
            {
                return ResultDto<bool>.Failure("فرمت درخواست غلط است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequest(purchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<bool>.Failure("درخواست مورد نظر یافت نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingCentralOffice)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت مناسبی نیست");
            }

            var fromStatus = purchaseRequest.Status;

            purchaseRequest.Status = PurchaseRequestStatus.RejectedByCentralOffice;

            var now = DateTime.UtcNow;

            purchaseRequest.History.Add(new PurchaseRequestHistory
            {
                PurchaseRequestId = purchaseRequest.Id,
                FromStatus = fromStatus,
                ToStatus = purchaseRequest.Status,
                ChangedAt = now,
                ChangedByUserId = currentUserService.UserId,
                CreatedAt = now,
                CreatedByUserId = currentUserService.UserId,
                Notes = "رد توسط دفتر مرکزی"
            });

            var result = await unitOfWork.SaveChangesAsync(ct) > 0;

            if (!result)
            {
                return ResultDto<bool>.Failure("مشکل در بروزرسانی درخواست خرید");
            }

            return ResultDto<bool>.Success("درخواست خرید با موفقیت رد شد");
        }
        public async Task<ResultDto<GetPurchaseRequestDto>> GetPurchaseRequestDetailForProcurementAsync(int purchaseRequestId, CancellationToken ct)
        {
            if (purchaseRequestId <= 0)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("شناسه درخواست نامعتبر است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequestWithDetail(purchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("درخواست خرید یافت نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingProcurement)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("درخواست در وضعیت خرید نیست");
            }

            return ResultDto<GetPurchaseRequestDto>.Success("درخواست خرید با موفقیت دریافت شد",purchaseRequest);
        }

        public async Task<ResultDto<bool>> ProcessPurchaseAsync(ProcessPurchaseDto processPurchaseDto, CancellationToken ct)
        {
            if (processPurchaseDto == null || processPurchaseDto.Items == null || !processPurchaseDto.Items.Any())
            {
                return ResultDto<bool>.Failure("اطلاعات خرید نامعتبر است");
            }

            if (string.IsNullOrWhiteSpace(processPurchaseDto.Supplier))
            {
                return ResultDto<bool>.Failure("تامین کننده الزامی است");
            }

            if (processPurchaseDto.ExpectedDeliveryDate < processPurchaseDto.PurchaseDate)
            {
                return ResultDto<bool>.Failure("تاریخ تحویل نمی‌تواند قبل از تاریخ خرید باشد");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequest(processPurchaseDto.PurchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<bool>.Failure("درخواست خرید یافت نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingProcurement)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت خرید نیست");
            }

            var itemIds = processPurchaseDto.Items
                .Select(i => i.PurchaseRequestItemId)
                .ToList();

            if (itemIds.Distinct().Count() != itemIds.Count)
            {
                return ResultDto<bool>.Failure("اقلام تکراری در درخواست وجود دارد");
            }

            if (purchaseRequest.Items.Count != itemIds.Count ||
                purchaseRequest.Items.Any(i => !itemIds.Contains(i.Id)))
            {
                return ResultDto<bool>.Failure("اطلاعات اقلام خرید نامعتبر است");
            }

            var itemsById = processPurchaseDto.Items
                .ToDictionary(i => i.PurchaseRequestItemId);

            foreach (var item in purchaseRequest.Items)
            {
                if (!itemsById.TryGetValue(item.Id, out var submittedItem))
                {
                    return ResultDto<bool>.Failure("اطلاعات اقلام درخواست نامعتبر است");
                }

                if (submittedItem.QuantityPurchased < 0 ||
                    submittedItem.QuantityPurchased > item.QuantityApproved)
                {
                    return ResultDto<bool>.Failure("مقدار خریداری شده نامعتبر است");
                }

                item.QuantityPurchased = submittedItem.QuantityPurchased;
            }

            if (purchaseRequest.Items.All(i => i.QuantityPurchased == 0))
            {
                return ResultDto<bool>.Failure("حداقل یک کالا باید خریداری شود");
            }

            var now = DateTime.UtcNow;
            var fromStatus = purchaseRequest.Status;

            purchaseRequest.Supplier = processPurchaseDto.Supplier.Trim();
            purchaseRequest.PurchaseDate = processPurchaseDto.PurchaseDate;
            purchaseRequest.ExpectedDeliveryDate = processPurchaseDto.ExpectedDeliveryDate;
            purchaseRequest.Notes = processPurchaseDto.Notes;

            purchaseRequest.Status = PurchaseRequestStatus.PendingWarehouseReceipt;

            purchaseRequest.UpdatedAt = now;
            purchaseRequest.UpdatedByUserId = currentUserService.UserId;

            purchaseRequest.History.Add(new PurchaseRequestHistory
            {
                PurchaseRequestId = purchaseRequest.Id,
                FromStatus = fromStatus,
                ToStatus = purchaseRequest.Status,
                ChangedAt = now,
                ChangedByUserId = currentUserService.UserId,
                CreatedAt = now,
                CreatedByUserId = currentUserService.UserId,
                Notes = "خرید توسط واحد تدارکات ثبت شد"
            });

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در ثبت خرید");
            }

            return ResultDto<bool>.Success("خرید با موفقیت ثبت شد", true);
        }

        public async Task<ResultDto<bool>> ConfirmWarehouseReceiptAsync(ConfirmPurchaseReceiptDto dto, CancellationToken ct)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                return ResultDto<bool>.Failure("اطلاعات دریافت نامعتبر است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequestForWarehouseReceiptAsync(dto.PurchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<bool>.Failure("درخواست خرید یافت نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingWarehouseReceipt)
            {
                return ResultDto<bool>.Failure("درخواست در وضعیت دریافت انبار نیست");
            }

            var request = purchaseRequest.Request;
            var warehouse = request.Warehouse;

            if (warehouse == null)
            {
                return ResultDto<bool>.Failure("انبار مربوط به درخواست یافت نشد");
            }

            var warehouseStockLocation = warehouse.StockLocation;

            if (warehouseStockLocation == null)
            {
                return ResultDto<bool>.Failure("محل موجودی انبار یافت نشد");
            }

            var purchasedItems = purchaseRequest.Items
                .Where(i => i.QuantityPurchased > 0)
                .ToList();

            if (!purchasedItems.Any())
            {
                return ResultDto<bool>.Failure("هیچ کالای خریداری شده‌ای یافت نشد");
            }

            var itemIds = dto.Items
                .Select(i => i.PurchaseRequestItemId)
                .ToList();

            if (itemIds.Distinct().Count() != itemIds.Count)
            {
                return ResultDto<bool>.Failure("اقلام تکراری در درخواست وجود دارد");
            }

            var itemIdSet = itemIds.ToHashSet();

            if (purchasedItems.Count != itemIds.Count ||
                purchasedItems.Any(i => !itemIdSet.Contains(i.Id)))
            {
                return ResultDto<bool>.Failure("اطلاعات اقلام نامعتبر است");
            }

            var itemsById = dto.Items
                .ToDictionary(i => i.PurchaseRequestItemId);

            var receivedItems = new Dictionary<int, decimal>();

            foreach (var item in purchasedItems)
            {
                if (!itemsById.TryGetValue(item.Id, out var submittedItem))
                {
                    return ResultDto<bool>.Failure("اطلاعات دریافت یکی از اقلام یافت نشد");
                }

                var quantityReceivedNow = submittedItem.QuantityReceivedNow;

                if (quantityReceivedNow < 0)
                {
                    return ResultDto<bool>.Failure("مقدار دریافت شده نامعتبر است");
                }

                if (item.QuantityReceived + quantityReceivedNow > item.QuantityPurchased)
                {
                    return ResultDto<bool>.Failure("مقدار دریافت شده نمی‌تواند بیشتر از مقدار خریداری شده باشد");
                }

                if (quantityReceivedNow > 0)
                {
                    receivedItems.Add(item.Id, quantityReceivedNow);
                }
            }

            if (!receivedItems.Any())
            {
                return ResultDto<bool>.Failure("حداقل مقدار دریافت یکی از اقلام باید بیشتر از صفر باشد");
            }

            var productIds = purchasedItems
                .Where(i => receivedItems.ContainsKey(i.Id))
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var inventoryBalances = await inventoryRepository
                .GetInventoryBalancesForUpdateAsync(productIds, warehouseStockLocation.Id, ct);

            var inventoryByProductId = inventoryBalances.ToDictionary(i => i.ProductId);

            var newInventoryBalances = new List<InventoryBalance>();

            var now = DateTime.UtcNow;

            foreach (var item in purchasedItems)
            {
                if (!receivedItems.TryGetValue(item.Id, out var quantityReceivedNow))
                {
                    continue;
                }

                if (inventoryByProductId.TryGetValue(item.ProductId, out var inventory))
                {
                    inventory.Quantity += quantityReceivedNow;
                    inventory.LastUpdatedAt = now;
                    inventory.UpdatedAt = now;
                    inventory.UpdatedByUserId = currentUserService.UserId;
                }
                else
                {
                    var newInventory = new InventoryBalance
                    {
                        ProductId = item.ProductId,
                        StockLocationId = warehouseStockLocation.Id,
                        Quantity = quantityReceivedNow,
                        LastUpdatedAt = now,
                        CreatedAt = now,
                        CreatedByUserId = currentUserService.UserId,
                        IsDeleted = false
                    };

                    newInventoryBalances.Add(newInventory);
                    inventoryByProductId.Add(item.ProductId, newInventory);
                }

                item.QuantityReceived += quantityReceivedNow;

                item.UpdatedAt = now;
                item.UpdatedByUserId = currentUserService.UserId;
            }

            if (newInventoryBalances.Any())
            {
                await inventoryRepository.AddInventoryBalancesAsync(newInventoryBalances, ct);
            }

            var fullyReceived = purchasedItems.All(i =>
                i.QuantityReceived >= i.QuantityPurchased);

            if (fullyReceived)
            {
                var purchaseRequestFromStatus = purchaseRequest.Status;
                var requestFromStatus = request.Status;

                purchaseRequest.Status = PurchaseRequestStatus.Completed;

                request.Status = RequestStatus.PendingWarehouse;
                request.UpdatedAt = now;
                request.UpdatedByUserId = currentUserService.UserId;

                purchaseRequest.History.Add(new PurchaseRequestHistory
                {
                    PurchaseRequestId = purchaseRequest.Id,
                    FromStatus = purchaseRequestFromStatus,
                    ToStatus = purchaseRequest.Status,
                    ChangedAt = now,
                    ChangedByUserId = currentUserService.UserId,
                    CreatedAt = now,
                    CreatedByUserId = currentUserService.UserId,
                    Notes = "تمام اقلام خریداری شده توسط انبار دریافت شد"
                });

                request.History.Add(new RequestHistory
                {
                    RequestId = request.Id,
                    FromStatus = requestFromStatus,
                    ToStatus = request.Status,
                    ChangedAt = now,
                    ChangedByUserId = currentUserService.UserId,
                    CreatedAt = now,
                    CreatedByUserId = currentUserService.UserId,
                    Notes = "خرید تکمیل شد و درخواست جهت ارسال کالا به انبار ارجاع شد"
                });
            }

            purchaseRequest.UpdatedAt = now;
            purchaseRequest.UpdatedByUserId = currentUserService.UserId;

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                return ResultDto<bool>.Failure("خطا در ثبت دریافت کالا");
            }

            if (fullyReceived)
            {
                return ResultDto<bool>.Success("تمام اقلام خریداری شده با موفقیت دریافت شدند", true);
            }

            return ResultDto<bool>.Success("دریافت کالا با موفقیت ثبت شد", true);
        }

        public async Task<ResultDto<GetPurchaseRequestDto>> GetPurchaseRequestDetailForWarehouseReceiptAsync(int purchaseRequestId, CancellationToken ct)
        {
            if (purchaseRequestId <= 0)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("شناسه درخواست نامعتبر است");
            }

            var purchaseRequest = await purchaseRequestRepository.GetPurchaseRequestWithDetail(purchaseRequestId, ct);

            if (purchaseRequest == null)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("درخواست خرید یافت نشد");
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.PendingWarehouseReceipt)
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("درخواست در وضعیت دریافت انبار نیست");
            }

            purchaseRequest.Items = purchaseRequest.Items
                .Where(i => i.QuantityToPurchase > 0)
                .ToList();

            if (!purchaseRequest.Items.Any())
            {
                return ResultDto<GetPurchaseRequestDto>.Failure("هیچ کالای خریداری شده‌ای یافت نشد");
            }

            return ResultDto<GetPurchaseRequestDto>.Success(
                "درخواست خرید با موفقیت دریافت شد",
                purchaseRequest);
        }
    }
}
