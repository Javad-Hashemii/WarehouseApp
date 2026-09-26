using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class PurchaseRequestHistoryConfiguration : IEntityTypeConfiguration<PurchaseRequestHistory>
    {
        public void Configure(EntityTypeBuilder<PurchaseRequestHistory> builder)
        {
            builder.HasKey(prh => prh.Id);

            builder.Property(prh => prh.FromStatus)
                .IsRequired();

            builder.Property(prh => prh.ToStatus)
                .IsRequired();

            builder.Property(prh => prh.ChangedAt)
                .IsRequired();

            builder.Property(prh => prh.Notes)
                .HasMaxLength(1000);

            builder.HasOne(prh => prh.PurchaseRequest)
                .WithMany(pr => pr.History)
                .HasForeignKey(prh => prh.PurchaseRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(prh => prh.ChangedByUser)
                .WithMany(u=>u.PurchaseRequestHistories)
                .HasForeignKey(prh => prh.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
