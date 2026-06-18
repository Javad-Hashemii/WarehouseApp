using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Workshop;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IWorkshopRepository
    {
        Task<bool> CreateWorkshopAsync(string name, int cityId, CancellationToken ct);
        Task<bool> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct);
        Task<List<GetAllWorkshopsDto>> GetWorkshopsAsync(CancellationToken ct);
        Task<bool> IsWorkshopExistAsync(string workshopName, CancellationToken ct);


    }
}
