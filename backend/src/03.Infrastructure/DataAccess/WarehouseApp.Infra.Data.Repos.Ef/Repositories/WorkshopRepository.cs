using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Workshop;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class WorkshopRepository(AppDbContext _context) : IWorkshopRepository
    {
        public async Task<bool> CreateWorkshopAsync(string name, int cityId, CancellationToken ct)
        {
            var workshop = new Workshop
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
                .Where(w => !w.IsDeleted)
                .Select(w => new GetAllWorkshopsDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                    CityName=w.City.Name
                    
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto, CancellationToken ct)
        {
            var result = await _context.Workshops
                .Where(w => w.Id == updateWorkshopDto.Id && !w.IsDeleted)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(w => w.Name, updateWorkshopDto.Name)
                    .SetProperty(w => w.CityId, updateWorkshopDto.CityId), ct);
            return result > 0;
        }
        public async Task<bool> IsWorkshopExistAsync(string workshopName, CancellationToken ct)
        {
            return await _context.Workshops.AnyAsync(w => w.Name == workshopName && !w.IsDeleted, ct);
        }

        public async Task<GetWorkshopDetailDto?> GetWorkshopDetailAsync(int workshopId, CancellationToken ct)
        {
            return await _context.Workshops
                .AsNoTracking()
                .Where(w => w.Id == workshopId && !w.IsDeleted)
                .Select(w => new GetWorkshopDetailDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                    CityName = w.City.Name

                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsWorkshopExistByIdAsync(int workshopId, CancellationToken ct)
        {
            return await _context.Workshops.AnyAsync(w => w.Id == workshopId&& !w.IsDeleted, ct);
        }

        public async Task<bool> SoftDeleteWorkshopAsync(int workshopId, int deletedByUserId, CancellationToken ct)
        {
            var result = await _context.Workshops
                .Where(w => w.Id == workshopId&& !w.IsDeleted)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(w => w.IsDeleted, true)
                    .SetProperty(w => w.DeletedByUserId, deletedByUserId)
                    .SetProperty(w => w.DeletedAt, DateTime.UtcNow), ct);
            return result > 0;
        }
    }
}
