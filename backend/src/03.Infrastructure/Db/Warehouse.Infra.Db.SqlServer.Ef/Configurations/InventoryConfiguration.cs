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

            builder.HasOne(ib => ib.Product)
                .WithMany()
                .HasForeignKey(ib => ib.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.Warehouse)
                .WithMany(w => w.InventoryBalances)
                .HasForeignKey(ib => ib.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ib => new { ib.ProductId, ib.WarehouseId })
                .IsUnique();
        }
    }
}
