using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.PurchaseRequest;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IPurchaseRequestRepository
    {
        Task AddAsync(PurchaseRequest purchaseRequest, CancellationToken ct);
        Task<List<GetAllPurchaseRequestsDto>> GetSummariesByStatusesAsync(IReadOnlyCollection<PurchaseRequestStatus> statuses,CancellationToken ct);

        Task<GetPurchaseRequestDto?> GetPurchaseRequestWithDetail(int purchaseRequest, CancellationToken ct);

        Task<PurchaseRequest?> GetPurchaseRequest(int purchaseRequestId, CancellationToken ct);

        Task<bool> DenyPurchaseRequestAsync(int purchaseRequestId, CancellationToken ct);
        Task<bool> IsPurchaseRequestExistsAsync(int purchaseRequestId, CancellationToken ct);

        Task<PurchaseRequest?>GetPurchaseRequestForWarehouseReceiptAsync(int purchaseRequestId, CancellationToken ct);

    }
}
