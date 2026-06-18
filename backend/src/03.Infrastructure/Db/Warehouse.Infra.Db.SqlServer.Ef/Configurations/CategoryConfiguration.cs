using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.ImagPath)
                .IsRequired(false);

            // Index on Name for faster lookups
            builder.HasIndex(c => c.Name);

            // Relationship: Category -> SubCategories
            builder.HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if SubCategories exist

            // Relationship: Category -> Products
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
               new Category { Id = 1, Name = "مصالح ساختمانی" },
               new Category { Id = 2, Name = "ابزار و تجهیزات" },
               new Category { Id = 3, Name = "تاسیسات برقی" },
               new Category { Id = 4, Name = "تاسیسات مکانیکی" },
               new Category { Id = 5, Name = "رنگ و پوشش" },
               new Category { Id = 6, Name = "ایمنی و حفاظت فردی" }
               );
        }
    }
}
