using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class WorkshopConfiguration : IEntityTypeConfiguration<Workshop>
    {
        public void Configure(EntityTypeBuilder<Workshop> builder)
        {

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(w => new { w.Name, w.CityId })
                .IsUnique();

            builder.HasOne(w => w.City)
                .WithMany(c => c.Workshops)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                new Workshop { Id = 1, Name = "کارگاه ساختمانی شرق", CityId = 1 },
                new Workshop { Id = 2, Name = "کارگاه تاسیسات مرکزی", CityId = 1 },
                new Workshop { Id = 3, Name = "کارگاه برق و مکانیک", CityId = 2 },
                new Workshop { Id = 4, Name = "کارگاه عمرانی جنوب", CityId = 3 }
            );
        }
    }
}
