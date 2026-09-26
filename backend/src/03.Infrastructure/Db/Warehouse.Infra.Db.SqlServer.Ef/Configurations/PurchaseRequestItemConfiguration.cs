using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class PurchaseRequestItemConfiguration : IEntityTypeConfiguration<PurchaseRequestItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseRequestItem> builder)
        {
            builder.HasKey(pri => pri.Id);


            builder.Property(pri => pri.QuantityRequested)
                .HasPrecision(18, 3);

            builder.Property(pri => pri.QuantityApproved)
                .HasPrecision(18, 3)
                .HasDefaultValue(0);

            builder.Property(pri => pri.QuantityReceived)
                .HasPrecision(18, 3)
                .HasDefaultValue(0);

            builder.Property(pri => pri.QuantityPurchased)
                .HasPrecision(18, 3)
                .HasDefaultValue(0);

            builder.HasOne(pri => pri.PurchaseRequest)
                .WithMany(pr => pr.Items)
                .HasForeignKey(pri => pri.PurchaseRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pri => pri.Product)
                .WithMany()
                .HasForeignKey(pri => pri.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pri => pri.RequestItem)
                .WithMany()
                .HasForeignKey(pri => pri.RequestItemId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
