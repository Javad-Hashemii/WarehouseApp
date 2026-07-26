using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Id);

            builder.HasQueryFilter(p => !p.IsDeleted);


            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Unit)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.IsConsumable)
                .HasDefaultValue(true);

            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => new { p.CategoryId, p.SubCategoryId });

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // Seed Data
            builder.HasData(
                // سیمان و ملات
                new Product { Id = 1, CategoryId = 1, SubCategoryId = 1, Name = "سیمان پرتلند تیپ 2", Unit = "کیسه", IsConsumable = true },
                new Product { Id = 2, CategoryId = 1, SubCategoryId = 1, Name = "ملات آماده", Unit = "کیسه", IsConsumable = true },

                // آجر و بلوک
                new Product { Id = 3, CategoryId = 1, SubCategoryId = 2, Name = "آجر سفال 5 سانت", Unit = "عدد", IsConsumable = true },
                new Product { Id = 4, CategoryId = 1, SubCategoryId = 2, Name = "بلوک سبک 20 سانت", Unit = "عدد", IsConsumable = true },

                // میلگرد و آهن آلات
                new Product { Id = 5, CategoryId = 1, SubCategoryId = 3, Name = "میلگرد آجدار A3 قطر 12", Unit = "شاخه", IsConsumable = true },
                new Product { Id = 6, CategoryId = 1, SubCategoryId = 3, Name = "تیرآهن 16", Unit = "شاخه", IsConsumable = true },

                // ابزار دستی
                new Product { Id = 7, CategoryId = 2, SubCategoryId = 5, Name = "چکش معمولی 500 گرم", Unit = "عدد", IsConsumable = false },
                new Product { Id = 8, CategoryId = 2, SubCategoryId = 5, Name = "انبردست 10 اینچ", Unit = "عدد", IsConsumable = false },

                // ابزار برقی
                new Product { Id = 9, CategoryId = 2, SubCategoryId = 6, Name = "دریل برقی بوش", Unit = "دستگاه", IsConsumable = false },
                new Product { Id = 10, CategoryId = 2, SubCategoryId = 6, Name = "فرز آهنبری", Unit = "دستگاه", IsConsumable = false },

                // کابل و سیم
                new Product { Id = 11, CategoryId = 3, SubCategoryId = 8, Name = "کابل افشان 1.5*2", Unit = "متر", IsConsumable = true },
                new Product { Id = 12, CategoryId = 3, SubCategoryId = 8, Name = "سیم برق 2.5", Unit = "متر", IsConsumable = true },

                // لوله و اتصالات
                new Product { Id = 13, CategoryId = 4, SubCategoryId = 11, Name = "لوله گالوانیزه 3/4 اینچ", Unit = "شاخه", IsConsumable = true },
                new Product { Id = 14, CategoryId = 4, SubCategoryId = 11, Name = "زانو PVC 1 اینچ", Unit = "عدد", IsConsumable = true },

                // رنگ ساختمانی
                new Product { Id = 15, CategoryId = 5, SubCategoryId = 14, Name = "رنگ پلاستیک سفید", Unit = "گالن", IsConsumable = true },
                new Product { Id = 16, CategoryId = 5, SubCategoryId = 14, Name = "رنگ روغن", Unit = "کیلوگرم", IsConsumable = true },

                // کلاه ایمنی و کفش
                new Product { Id = 17, CategoryId = 6, SubCategoryId = 17, Name = "کلاه ایمنی استاندارد", Unit = "عدد", IsConsumable = false },
                new Product { Id = 18, CategoryId = 6, SubCategoryId = 17, Name = "کفش ایمنی چرم", Unit = "عدد", IsConsumable = false }
                );
        }
    }
}
