using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Workshop;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class WorkshopRepository(AppDbContext _context) : IWorkshopRepository
    {
        public async Task<bool> CreateWorkshopAsync(string name, int cityId, CancellationToken ct)
        {
            var workshop = new Domain.Core.Entities.Workshop
            {
                Name = name,
                CityId = cityId
            };
            _context.Workshops.Add(workshop);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<List<GetAllWorkshopsDto>> GetWorkshopsAsync(CancellationToken ct)
        {
            return await _context.Workshops
                .AsNoTracking()
                .Select(w => new GetAllWorkshopsDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct)
        {
            var result = _context.Workshops
                .Where(w => w.Id == updateWorkshopDto.Id)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(w => w.Name, updateWorkshopDto.Name)
                    .SetProperty(w => w.CityId, updateWorkshopDto.CityId), ct);
            return await result > 0;
        }
        public async Task<bool> IsWorkshopExistAsync(string workshopName, CancellationToken ct)
        {
            return await _context.Workshops.AnyAsync(w => w.Name == workshopName, ct);
        }
    }
}
