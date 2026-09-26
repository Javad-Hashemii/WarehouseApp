using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.PurchaseRequest;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IPurchaseRequestService
    {
        Task<ResultDto<List<GetAllPurchaseRequestsDto>>> GetPurchaseRequestsForCurrentUserAsync(CancellationToken ct);
        Task<ResultDto<GetPurchaseRequestDto>> GetPurchaseRequestDetailForCentralManagerAsync(int requestId, CancellationToken ct);
        Task<ResultDto<bool>> ApprovePurchaseRequestAsync(ApprovePurchaseRequestDto approvePurchaseRequestDto, CancellationToken ct);
        Task<ResultDto<bool>> DenyPurchaseRequestAsync(int purchaseRequestId, CancellationToken ct);
        Task<ResultDto<GetPurchaseRequestDto>> GetPurchaseRequestDetailForProcurementAsync(int purchaseRequestId, CancellationToken ct);
        Task<ResultDto<bool>> ProcessPurchaseAsync(ProcessPurchaseDto processPurchaseDto, CancellationToken ct);
        Task<ResultDto<bool>> ConfirmWarehouseReceiptAsync(ConfirmPurchaseReceiptDto dto,CancellationToken ct);
        Task<ResultDto<GetPurchaseRequestDto>> GetPurchaseRequestDetailForWarehouseReceiptAsync(int purchaseRequestId, CancellationToken ct);

    }
}
