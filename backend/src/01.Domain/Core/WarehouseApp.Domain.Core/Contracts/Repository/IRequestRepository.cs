using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
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

        Task<List<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status, CancellationToken ct);

        Task<GetRequestDto?> GetByIdWithDetailsAsync(int id, CancellationToken ct);

        Task<List<RequestItem>> GetRequestItemsByIdsAsync(int requestId, IReadOnlyCollection<int> itemIds, CancellationToken ct);
        Task AddRequestItemsAsync(IEnumerable<RequestItem> items, CancellationToken ct);
        void RemoveRequestItems(IEnumerable<RequestItem> items);
        Task<int> SaveChangesAsync(CancellationToken ct);
        Task<bool> IsRequestNumberExistAsync(string requestNumber, CancellationToken ct);

        Task<Request?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<GetAllRequestsDto>> GetAllSummariesAsync(CancellationToken ct);



    }
}
