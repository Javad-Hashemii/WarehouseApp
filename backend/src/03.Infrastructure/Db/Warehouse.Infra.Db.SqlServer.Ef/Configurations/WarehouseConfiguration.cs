using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(w => new { w.Name, w.CityId })
                .IsUnique();

            builder.HasOne(w => w.City)
                .WithMany(c => c.Warehouses)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.InventoryBalances)
                .WithOne(ib => ib.Warehouse)
                .HasForeignKey(ib => ib.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.InventoryTransactions)
                .WithOne(it => it.Warehouse)
                .HasForeignKey(it => it.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.Requests)
                .WithOne(r => r.Warehouse)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                new Warehouse { Id = 1, Name = "انبار مرکزی ماهشهر", CityId = 1 },
                new Warehouse { Id = 2, Name = "انبار بهبهان", CityId = 1 },
                new Warehouse { Id = 3, Name = "انبار اصفهان", CityId = 2 },
                new Warehouse { Id = 4, Name = "انبار شیراز", CityId = 3 }
            );
        }
    }
}
