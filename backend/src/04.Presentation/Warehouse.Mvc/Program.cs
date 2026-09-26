using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.Mvc.Services;
using WarehouseApp.Application;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Contracts.UnitOfWork;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Data.Repos.Ef;
using WarehouseApp.Infra.Db.SqlServer.Ef;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var siteSettings = builder.Configuration
    .GetSection("SiteSettings")
    .Get<SiteSettings>()
    ?? throw new InvalidOperationException("SiteSettings section not found");
builder.Services.AddSingleton(siteSettings);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
builder.Services.AddApplicationServices();
builder.Services.AddDataAccessEf(siteSettings);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRequestDraftService, RequestDraftService>();
builder.Services.AddScoped<IEditRequestDraftService, EditRequestDraftService>();
builder.Services.AddScoped<IPurchaseDecisionDraftService, PurchaseDecisionDraftService>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(
    options =>
    {
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
