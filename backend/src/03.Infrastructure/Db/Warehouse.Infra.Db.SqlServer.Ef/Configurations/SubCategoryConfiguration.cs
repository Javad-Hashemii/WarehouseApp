using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {

            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(sc => sc.ImagePath)
                .IsRequired(false);

            builder.HasIndex(sc => new { sc.CategoryId, sc.Name });

            builder.HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sc => sc.Products)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                // مصالح ساختمانی
                new SubCategory { Id = 1, CategoryId = 1, Name = "سیمان و ملات" },
                new SubCategory { Id = 2, CategoryId = 1, Name = "آجر و بلوک" },
                new SubCategory { Id = 3, CategoryId = 1, Name = "میلگرد و آهن آلات" },
                new SubCategory { Id = 4, CategoryId = 1, Name = "شن و ماسه" },

                // ابزار و تجهیزات
                new SubCategory { Id = 5, CategoryId = 2, Name = "ابزار دستی" },
                new SubCategory { Id = 6, CategoryId = 2, Name = "ابزار برقی" },
                new SubCategory { Id = 7, CategoryId = 2, Name = "داربست و نردبان" },

                // تاسیسات برقی
                new SubCategory { Id = 8, CategoryId = 3, Name = "کابل و سیم" },
                new SubCategory { Id = 9, CategoryId = 3, Name = "کلید و پریز" },
                new SubCategory { Id = 10, CategoryId = 3, Name = "تابلو برق" },

                // تاسیسات مکانیکی
                new SubCategory { Id = 11, CategoryId = 4, Name = "لوله و اتصالات" },
                new SubCategory { Id = 12, CategoryId = 4, Name = "شیرآلات" },
                new SubCategory { Id = 13, CategoryId = 4, Name = "پمپ و الکتروپمپ" },

                // رنگ و پوشش
                new SubCategory { Id = 14, CategoryId = 5, Name = "رنگ ساختمانی" },
                new SubCategory { Id = 15, CategoryId = 5, Name = "رنگ صنعتی" },
                new SubCategory { Id = 16, CategoryId = 5, Name = "عایق‌های رطوبتی" },

                // ایمنی و حفاظت فردی
                new SubCategory { Id = 17, CategoryId = 6, Name = "کلاه ایمنی و کفش" },
                new SubCategory { Id = 18, CategoryId = 6, Name = "دستکش و عینک" },
                new SubCategory { Id = 19, CategoryId = 6, Name = "لباس کار" }
            );
        }
    }
}
