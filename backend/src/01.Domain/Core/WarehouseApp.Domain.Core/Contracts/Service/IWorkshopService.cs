using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Workshop;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IWorkshopService
    {
        Task<ResultDto<bool>> CreateWorkshopAsync(CreateWorkshopDto createWorkshopDto, CancellationToken ct);
        Task<ResultDto<bool>> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct);
        Task<List<GetAllWorkshopsDto>> GetWorkshopsAsync(CancellationToken ct);
    }
}
