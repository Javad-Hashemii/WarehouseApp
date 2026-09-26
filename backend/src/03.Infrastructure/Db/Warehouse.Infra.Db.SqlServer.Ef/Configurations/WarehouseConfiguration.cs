using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(w => new
            {
                w.Name,
                w.CityId
            })
            .IsUnique();

            builder.HasOne(w => w.City)
                .WithMany(c => c.Warehouses)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Workshop)
                .WithOne(ws => ws.Warehouse)
                .HasForeignKey<Warehouse>(w => w.WorkshopId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.Requests)
                .WithOne(r => r.Warehouse)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Warehouse
                {
                    Id = 1,
                    Name = "انبار مرکزی ماهشهر",
                    CityId = 1,
                    WorkshopId = 1
                },
                new Warehouse
                {
                    Id = 2,
                    Name = "انبار بهبهان",
                    CityId = 1,
                    WorkshopId = 2
                },
                new Warehouse
                {
                    Id = 3,
                    Name = "انبار اصفهان",
                    CityId = 2,
                    WorkshopId = 3
                },
                new Warehouse
                {
                    Id = 4,
                    Name = "انبار شیراز",
                    CityId = 3,
                    WorkshopId = 4
                }
            );
        }
    }
}