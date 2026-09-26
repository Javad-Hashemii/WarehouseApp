using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
    {
        public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
        {
            builder.HasKey(pr=>pr.Id);

            builder.Property(pr => pr.PurchaseRequestNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(pr => pr.Request)
                .WithMany()
                .HasForeignKey(pr => pr.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pr => pr.Workshop)
                .WithMany()
                .HasForeignKey(pr => pr.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pr => pr.RequestId)
                .IsUnique();
        }
    }
}
