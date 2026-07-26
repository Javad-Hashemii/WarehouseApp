using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
