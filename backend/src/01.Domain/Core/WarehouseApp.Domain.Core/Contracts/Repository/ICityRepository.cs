using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.City;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface ICityRepository
    {
        Task<bool> CreateCity(CreateCityDto createCityDto, CancellationToken ct);
        Task<List<GetAllCitiesDto>> GetAllCities(CancellationToken ct);
        Task<bool> UpdateCity(int cityId, int userId, string name, CancellationToken ct);
        Task<bool> DeleteCity(string name, CancellationToken ct);
        Task<bool> IsCityExist(string name, CancellationToken ct);
    }
}
