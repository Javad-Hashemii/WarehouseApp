using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.City;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class CityRepository(AppDbContext _context) : ICityRepository
    {
        public async Task<bool> CreateCity(CreateCityDto createCityDto, CancellationToken ct)
        {
            var city = new City
            {
                Name = createCityDto.Name,
                CreatedAt = DateTime.Now,
                CreatedByUserId = createCityDto.UserId,
            };
            _context.Cities.Add(city);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public Task<bool> DeleteCity(string name, CancellationToken ct)
        {
            throw new NotImplementedException(); //implement soft delete?
        }

        public async Task<List<GetAllCitiesDto>> GetAllCities(CancellationToken ct)
        {
            return await _context.Cities.Select(c => new GetAllCitiesDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync(ct);
        }

        public async Task<bool> UpdateCity(int cityId,int userId, string name, CancellationToken ct)
        {
            var result =  _context.Cities
                .Where(c => c.Id == cityId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.Name, name)
                .SetProperty(c => c.UpdatedAt, DateTime.Now)
                .SetProperty(c => c.UpdatedByUserId, userId), ct);

            return await result > 0;
        }

        public async Task<bool> IsCityExist(string name, CancellationToken ct)
        {
            return await _context.Cities.AnyAsync(c => c.Name == name, ct);
        }
}
}
