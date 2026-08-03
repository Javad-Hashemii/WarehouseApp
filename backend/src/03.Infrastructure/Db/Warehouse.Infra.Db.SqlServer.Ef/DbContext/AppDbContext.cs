using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.DbContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):IdentityDbContext<ApplicationUser,IdentityRole<int> ,int>(options)
    {
        public DbSet<Category> Categories {  get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<InventoryBalance> InventoryBalances { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<RequestHistory> RequestHistories { get; set; }
        public DbSet<StockLocation> StockLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
