using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IRequestService
    {
        Task<ResultDto<bool>> CreateRequestAsync(CreateRequestDto createRequestDto, CancellationToken ct);

        Task<ResultDto<bool>> UpdateRequestAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct);

        Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct);

        Task<ResultDto<bool>> UpdateStatusAsync(UpdateRequestStatusDto updateRequestStatusDto,CancellationToken ct);

        Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId,CancellationToken ct);

        Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId, CancellationToken ct);

        Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status,CancellationToken ct);

        Task<ResultDto<GetRequestDto?>> GetByIdWithDetailsAsync(int id, CancellationToken ct);
        Task<ResultDto<bool>> ModifyRequestItemsAsync(int requestId, IReadOnlyCollection<AddRequestItemDto> newItems, IReadOnlyCollection<UpdateRequestItemDto> updatedItems, IReadOnlyCollection<int> deletedItemIds, CancellationToken ct);
        Task<ResultDto<List<GetAllRequestsDto>>> GetRequestsForCurrentUserAsync(CancellationToken ct);
        //Task<ResultDto<bool>> ApproveWorkshopRequestAsync(int requestId, int userId, CancellationToken ct);
        Task<ResultDto<bool>> ApproveRequestAsync(int requestId, CancellationToken ct);
        Task<ResultDto<bool>> RejectRequestAsync(int requestId, CancellationToken ct);



    }
}
