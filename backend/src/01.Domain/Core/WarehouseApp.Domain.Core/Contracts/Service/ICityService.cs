using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.City;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface ICityService
    {
        Task<ResultDto<bool>> CreateCity(CreateCityDto createCityDto, CancellationToken ct);
        Task<List<GetAllCitiesDto>> GetAllCities(CancellationToken ct);
        Task<ResultDto<bool>> UpdateCity(UpdateCityDto updateCityDto, CancellationToken ct);
        
    }
}
