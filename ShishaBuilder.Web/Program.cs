using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Business.Repositories;
using ShishaBuilder.Business.Repositories.HookahRepositories;
using ShishaBuilder.Business.Repositories.MasterRepositories;
using ShishaBuilder.Business.Repositories.TableRepositories;
using ShishaBuilder.Business.Repositories.HookahRepositories;
using ShishaBuilder.Business.Repositories.MasterRepositories;
using ShishaBuilder.Business.Repositories.OrderRepositories;
using ShishaBuilder.Business.Repositories.TableRepositories;
using ShishaBuilder.Business.Services.BlobServices;
using ShishaBuilder.Business.Services.HookahServices;
using ShishaBuilder.Business.Services.HookahServices;
using ShishaBuilder.Business.Services.MasterServices;
using ShishaBuilder.Business.Services.OrderServices;
using ShishaBuilder.Business.Services.TobaccoServices;
using ShishaBuilder.Core.Repositories;
using ShishaBuilder.Core.Repositories.HookahRepositories;
using ShishaBuilder.Core.Repositories.HookahRepositories;
using ShishaBuilder.Core.Repositories.MasterRepositories;
using ShishaBuilder.Core.Repositories.TableRepositories;
using ShishaBuilder.Core.Repositories.OrderRepositories;
using ShishaBuilder.Core.Repositories.TableRepositories;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.OrderServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;
using ShishaBuilder.Core.Settings;
using ShishaBuilder.Core.Validation;
using ShishaBuilder.Core.Validation;
using ShishaBuilder.Core.Validation.HookahValidations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication("MyCookieAuthScheme")
    .AddCookie("MyCookieAuthScheme", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IHookahRepository, HookahRepository>();
builder.Services.AddScoped<IHookahService, HookahService>();

builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlobStorage"));

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IConfiguration>().GetSection("AzureBlobStorage").Get<BlobSettings>()
);


builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlobStorage"));

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IConfiguration>().GetSection("AzureBlobStorage").Get<BlobSettings>()
);


builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddDbContext<ShishaBuilder.Core.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDbContext<ShishaBuilder.Core.DB.AppDbContext>(options =>
{
    var coonectionString = builder.Configuration.GetConnectionString("DefaultConnectionJamal");
    options.UseNpgsql(coonectionString);
});

builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlobStorage"));
builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlobStorage"));

builder.Services.AddScoped<ITobaccoRepository, TobaccoRepository>();
builder.Services.AddScoped<ITobaccoService, TobaccoService>();


builder.Services.AddScoped<IMasterRepository, MasterRepository>();
builder.Services.AddScoped<IMasterService, MasterService>();


builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<CreateHookahValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateHookahValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<EditHookahValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTobaccoViewModelValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTableValidator>();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseDeveloperExceptionPage();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
