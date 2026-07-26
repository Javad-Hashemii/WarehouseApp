using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.UnitOfWork;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Db.SqlServer.Ef
{
    public class UnitOfWork(AppDbContext context):IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await context.SaveChangesAsync(ct);
        }
    }
}
