using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder.HasData(
                        new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "c7b013f0-5201-4317-abd8-c211f91b7330" },
                        new IdentityRole<int> { Id = 2, Name = "WarehouseManager", NormalizedName = "WAREHOUSEMANAGER", ConcurrencyStamp = "033fa693-010e-436f-a602-e0f0dfc836e4" },
                        new IdentityRole<int> { Id = 3, Name = "WorkshopManager", NormalizedName = "WORKSHOPMANAGER", ConcurrencyStamp = "5c52c6f6-49a3-4876-8805-5da845cce51c" },
                        new IdentityRole<int> { Id = 4, Name = "CentralManager", NormalizedName = "CENTRALMANAGER", ConcurrencyStamp = "81ba5de7-f4e7-4bd9-9fc4-8bdc87ccbf86" },
                        new IdentityRole<int> { Id = 5, Name = "ProcurementOfficer", NormalizedName = "PROCUREMENTOFFICER", ConcurrencyStamp = "d11b22e1-4545-4b7d-b3b3-8551da7ebf54" },
                        new IdentityRole<int> { Id = 6, Name = "WorkshopEmployee", NormalizedName = "WORKSHOPEMPLOYEE", ConcurrencyStamp = "e1f3c8d2-5b6a-4f9e-9c3b-1f2e3d4c5b6a" }
                    );
        }
    }
}
