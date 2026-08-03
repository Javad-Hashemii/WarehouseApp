using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(au => au.FullName)
                .HasMaxLength(100);

            builder.Property(au => au.IsActive)
                .HasDefaultValue(false);


            builder.HasOne(au => au.Workshop)
                .WithMany(w => w.WorkshopUsers)
                .HasForeignKey(au => au.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new ApplicationUser
                {
                    Id = 1,
                    FullName = "Omid Hashemi",
                    UserName = "omid@gmail.com",
                    NormalizedUserName = "OMID@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAENwIvr4Pn6t28/KI5Kh0/n+zAPGZGOKjq5iWt0sfzvdBRz/UaaiDbwNH2e5JbmDF0A==",
                    Email = "omid@gmail.com",
                    NormalizedEmail = "OMID@GMAIL.COM",
                    SecurityStamp = "f3a9e8b2-1234-4bcd-9f2a-abcdef123456",
                    ConcurrencyStamp = "6f4b8a3d-9f21-4b12-889c-123456abcdef" // static

                },
                new ApplicationUser
                {
                    Id = 67,
                    FullName = "Admin User",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAENwIvr4Pn6t28/KI5Kh0/n+zAPGZGOKjq5iWt0sfzvdBRz/UaaiDbwNH2e5JbmDF0A==",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    SecurityStamp= "f3a9e8b2-1234-4bcd-9f2a-abcdef123467",
                    ConcurrencyStamp = "6f4b8a3d-9f21-4b12-889c-123467abcdef", // static
                    IsActive = true
                    // static
                });
        }
    }
}
