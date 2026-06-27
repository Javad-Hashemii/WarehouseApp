using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(it => it.Id);

            builder.Property(it => it.Quantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(it => it.TransactionDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(it => it.TransactionType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(it => it.Notes)
                .HasMaxLength(500);

            builder.HasOne(it => it.Product)
                .WithMany()
                .HasForeignKey(it => it.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(it => it.SourceWarehouse)
                .WithMany()
                .HasForeignKey(it => it.SourceWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(it => it.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(it => it.DestinationWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(it => it.Request)
                .WithMany()
                .HasForeignKey(it => it.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(it => it.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(it => it.TransactionDate);

            builder.HasIndex(it => new
            {
                it.ProductId,
                it.SourceWarehouseId,
                it.DestinationWarehouseId,
                it.TransactionDate
            });
        }
    }
}
