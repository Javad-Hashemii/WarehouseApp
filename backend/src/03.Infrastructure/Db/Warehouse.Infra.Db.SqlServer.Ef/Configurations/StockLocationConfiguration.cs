using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class StockLocationConfiguration : IEntityTypeConfiguration<StockLocation>
    {
        public void Configure(EntityTypeBuilder<StockLocation> builder)
        {
            builder.HasKey(sl => sl.Id);

            builder.HasOne(sl => sl.Warehouse)
                .WithOne(w => w.StockLocation)
                .HasForeignKey<StockLocation>(sl => sl.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sl => sl.Workshop)
                .WithOne(w => w.StockLocation)
                .HasForeignKey<StockLocation>(sl => sl.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_StockLocation_ExactlyOneOwner",
                    """
                    ([WarehouseId] IS NOT NULL AND [WorkshopId] IS NULL)
                    OR
                    ([WarehouseId] IS NULL AND [WorkshopId] IS NOT NULL)
                    """));

            builder.HasData(
                // Warehouse stock locations
                new StockLocation
                {
                    Id = 1,
                    WarehouseId = 1,
                    WorkshopId = null
                },
                new StockLocation
                {
                    Id = 2,
                    WarehouseId = 2,
                    WorkshopId = null
                },
                new StockLocation
                {
                    Id = 3,
                    WarehouseId = 3,
                    WorkshopId = null
                },
                new StockLocation
                {
                    Id = 4,
                    WarehouseId = 4,
                    WorkshopId = null
                },

                // Workshop stock locations
                new StockLocation
                {
                    Id = 5,
                    WarehouseId = null,
                    WorkshopId = 1
                },
                new StockLocation
                {
                    Id = 6,
                    WarehouseId = null,
                    WorkshopId = 2
                },
                new StockLocation
                {
                    Id = 7,
                    WarehouseId = null,
                    WorkshopId = 3
                },
                new StockLocation
                {
                    Id = 8,
                    WarehouseId = null,
                    WorkshopId = 4
                }
            );
        }
    }
}
