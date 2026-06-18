using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(c => c.Workshops)
                .WithOne(w => w.City)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Warehouses)
                .WithOne(w => w.City)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new City { Id = 1, Name = "تهران" },
                new City { Id = 2, Name = "اصفهان" },
                new City { Id = 3, Name = "شیراز" },
                new City { Id = 4, Name = "مشهد" },
                new City { Id = 5, Name = "تبریز" },
                new City { Id = 6, Name = "اهواز" }
            );
        }
    }
}
