using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Application.Services;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;

namespace WarehouseApp.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRequestHistoryService, RequestHistoryService>();
            services.AddScoped<IRequestItemService, RequestItemService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWorkshopService, WorkshopService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
