using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class RequestHistoryConfiguration : IEntityTypeConfiguration<RequestHistory>
    {
        public void Configure(EntityTypeBuilder<RequestHistory> builder)
        {
            builder.HasKey(rh => rh.Id);



            builder.Property(rh => rh.ChangedByUserId)
                .IsRequired();

            builder.HasOne(rh => rh.ChangedByUser)
                .WithMany(au=>au.RequestHistories)
                .HasForeignKey(rh => rh.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
