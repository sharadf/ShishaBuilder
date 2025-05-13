using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Business.Repositories;
using ShishaBuilder.Business.Repositories.HookahRepositories;
using ShishaBuilder.Business.Repositories.MasterRepositories;
using ShishaBuilder.Business.Repositories.TableRepositories;
using ShishaBuilder.Business.Services.BlobServices;
using ShishaBuilder.Business.Services.HookahServices;
using ShishaBuilder.Business.Services.MasterServices;
using ShishaBuilder.Business.Services.TobaccoServices;
using ShishaBuilder.Core.Data;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories;
using ShishaBuilder.Core.Repositories.HookahRepositories;
using ShishaBuilder.Core.Repositories.MasterRepositories;
using ShishaBuilder.Core.Repositories.TableRepositories;
using ShishaBuilder.Core.Seeding;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;
using ShishaBuilder.Core.Settings;
using ShishaBuilder.Core.Validation.HookahValidations;


var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContextIdentity>()
    .AddDefaultTokenProviders();

builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });


builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContextIdentity>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionJamal");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IHookahRepository, HookahRepository>();
builder.Services.AddScoped<IHookahService, HookahService>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlobStorage"));
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<ITobaccoRepository, TobaccoRepository>();
builder.Services.AddScoped<ITobaccoService, TobaccoService>();

builder.Services.AddScoped<IMasterRepository, MasterRepository>();
builder.Services.AddScoped<IMasterService, MasterService>();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateHookahValidator>();

// builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddValidatorsFromAssemblyContaining<AdminRegistrationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MasterRegistrationValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AppDbSeeder.SeedRolesAsync(services);
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
