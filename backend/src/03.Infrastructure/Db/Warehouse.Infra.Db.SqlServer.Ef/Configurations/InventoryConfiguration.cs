using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<InventoryBalance>
    {
        public void Configure(EntityTypeBuilder<InventoryBalance> builder)
        {

            builder.HasKey(ib => ib.Id);

            builder.Property(ib => ib.Quantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(ib => ib.LastUpdatedAt)
                .IsRequired();

            builder.HasOne(ib => ib.Product)
                .WithMany()
                .HasForeignKey(ib => ib.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.StockLocation)
                .WithMany(sl => sl.InventoryBalances)
                .HasForeignKey(ib => ib.StockLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ib => new
            {
                ib.StockLocationId,
                ib.ProductId
            })
                .IsUnique();

            builder.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_InventoryBalance_Quantity_NonNegative",
                    "[Quantity] >= 0"));

            var seedDate = new DateTime(2026, 8, 3);

            builder.HasData(
                // Warehouse 1 - StockLocation 1
                new InventoryBalance
                {
                    Id = 1,
                    ProductId = 1,
                    StockLocationId = 1,
                    Quantity = 100m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },
                new InventoryBalance
                {
                    Id = 2,
                    ProductId = 11,
                    StockLocationId = 1,
                    Quantity = 500m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },

                // Warehouse 2 - StockLocation 2
                new InventoryBalance
                {
                    Id = 3,
                    ProductId = 1,
                    StockLocationId = 2,
                    Quantity = 75m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },
                new InventoryBalance
                {
                    Id = 4,
                    ProductId = 11,
                    StockLocationId = 2,
                    Quantity = 300m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },

                // Warehouse 3 - StockLocation 3
                new InventoryBalance
                {
                    Id = 5,
                    ProductId = 1,
                    StockLocationId = 3,
                    Quantity = 50m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },
                new InventoryBalance
                {
                    Id = 6,
                    ProductId = 11,
                    StockLocationId = 3,
                    Quantity = 200m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },

                // Warehouse 4 - StockLocation 4
                new InventoryBalance
                {
                    Id = 7,
                    ProductId = 1,
                    StockLocationId = 4,
                    Quantity = 40m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                },
                new InventoryBalance
                {
                    Id = 8,
                    ProductId = 11,
                    StockLocationId = 4,
                    Quantity = 150m,
                    LastUpdatedAt = seedDate,
                    CreatedAt = seedDate,
                    CreatedByUserId = 67,
                    IsDeleted = false
                }
            );


        }
    }
}
