using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IRequestRepository
    {
        Task<bool> CreateRequestAsync(CreateRequestDto createRequestDto , CancellationToken ct);

        Task<bool> UpdateRequestAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct);

        Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct);

        Task<bool> UpdateStatusAsync(UpdateRequestStatusDto updateRequestStatusDto,CancellationToken ct, string? notes = null);

        Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId, CancellationToken ct);

        Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId, CancellationToken ct);

        Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status, CancellationToken ct);

        Task<GetRequestDto?> GetByIdWithDetailsAsync(int id, CancellationToken ct);


    }
}
