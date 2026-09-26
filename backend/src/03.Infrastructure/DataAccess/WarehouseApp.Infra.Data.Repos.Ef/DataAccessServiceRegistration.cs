using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Infra.Data.Repos.Ef.Repositories;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef
{
    public static class DataAccessEfServiceRegistration
    {
        public static IServiceCollection AddDataAccessEf(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(siteSettings.ConnectionStrings.Sql);
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRequestHistoryRepository, RequestHistoryRepository>();
            services.AddScoped<IRequestItemRepository, RequestItemRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWorkshopRepository, WorkshopRepository>();
            services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();
            services.AddScoped<IPurchaseRequestHistoryRepository, PurchaseRequestHistoryRepository>();


            return services;
        }
    }
}
