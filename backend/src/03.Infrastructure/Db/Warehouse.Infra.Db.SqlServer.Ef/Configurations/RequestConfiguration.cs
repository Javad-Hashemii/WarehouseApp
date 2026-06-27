using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {

            builder.HasKey(r => r.Id);

            builder.Property(r => r.RequestNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(r => r.RequestNumber)
                .IsUnique();

            builder.Property(r => r.RequestDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(r => r.RequestDate);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(RequestStatus.Pending);

            builder.HasIndex(r => r.Status);

            builder.Property(r => r.Notes)
                .HasMaxLength(1000);

            builder.HasOne(r => r.RequestedByUser)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Workshop)
                .WithMany(w => w.Requests)
                .HasForeignKey(r => r.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Warehouse)
                .WithMany(w => w.Requests)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(r => r.RequestItems)
                .WithOne(ri => ri.Request)
                .HasForeignKey(ri => ri.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r=>r.History)
                .WithOne(h => h.Request)
                .HasForeignKey(h => h.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.WorkshopId, r.RequestDate });
        }
    }
}
