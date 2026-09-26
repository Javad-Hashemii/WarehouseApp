using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Quantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(t => t.QuantityReceived)
                .HasPrecision(18, 3);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.Notes)
                .HasMaxLength(1000);

            builder.HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.SourceStockLocation)
                .WithMany(sl => sl.OutgoingTransactions)
                .HasForeignKey(t => t.SourceStockLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationStockLocation)
                .WithMany(sl => sl.IncomingTransactions)
                .HasForeignKey(t => t.DestinationStockLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Request)
                .WithMany()
                .HasForeignKey(t => t.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.RequestItem)
                .WithMany(ri => ri.InventoryTransactions)
                .HasForeignKey(t => t.RequestItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.PerformedByUser)
                .WithMany()
                .HasForeignKey(t => t.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_InventoryTransaction_Quantity_Positive",
                    "[Quantity] > 0");

                table.HasCheckConstraint(
                    "CK_InventoryTransaction_HasLocation",
                    """
                    [SourceStockLocationId] IS NOT NULL
                    OR [DestinationStockLocationId] IS NOT NULL
                    """);

                table.HasCheckConstraint(
                    "CK_InventoryTransaction_DifferentLocations",
                    """
                    [SourceStockLocationId] IS NULL
                    OR [DestinationStockLocationId] IS NULL
                    OR [SourceStockLocationId] <> [DestinationStockLocationId]
                    """);

                table.HasCheckConstraint(
                    "CK_InventoryTransaction_QuantityReceived",
                    """
                    [QuantityReceived] IS NULL
                    OR (
                        [QuantityReceived] >= 0
                        AND [QuantityReceived] <= [Quantity]
                    )
                    """);
            });
        }
    }
}
