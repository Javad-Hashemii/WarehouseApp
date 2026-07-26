using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class RequestItemConfiguration : IEntityTypeConfiguration<RequestItem>
    {
        public void Configure(EntityTypeBuilder<RequestItem> builder)
        {

            builder.HasKey(ri => ri.Id);

            builder.Property(ri => ri.QuantityRequested)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(ri => ri.QuantityIssued)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(ri => ri.QuantityApproved)
                .HasPrecision(18, 3);

            builder.Property(ri => ri.QuantityReceived)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(ri => ri.Notes)
                .HasMaxLength(500);



            builder.HasOne(ri => ri.Request)
                .WithMany(r => r.RequestItems)
                .HasForeignKey(ri => ri.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ri => ri.Product)
                .WithMany()
                .HasForeignKey(ri => ri.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ri => new { ri.RequestId, ri.ProductId })
                .IsUnique();
        }
    }
}
