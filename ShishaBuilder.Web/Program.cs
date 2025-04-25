using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Business.Repositories;
using ShishaBuilder.Core.Validation;

using ShishaBuilder.Business.Services.BlobServices;
using ShishaBuilder.Business.Services.MasterServices;
using ShishaBuilder.Business.Services.TobaccoServices;
using ShishaBuilder.Core.Repositories;
using ShishaBuilder.Core.Repositories.MasterRepositories;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.TobaccoServices;
using ShishaBuilder.Core.Settings;
using ShishaBuilder.Core.Validation.HookahValidations;
using ShishaBuilder.Core.Repositories.HookahRepositories;
using ShishaBuilder.Business.Services.HookahServices;
using ShishaBuilder.Business.Repositories.HookahRepositories;
using ShishaBuilder.Business.Repositories.MasterRepositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IHookahRepository, HookahRepository>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<IHookahService, HookahService>();

builder.Services.AddDbContext<ShishaBuilder.Core.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<ShishaBuilder.Core.DB.AppDbContext>(options=>{
    var coonectionString= builder.Configuration.GetConnectionString("DefaultConnectionJamal");
    options.UseNpgsql(coonectionString);
});

builder.Services.Configure<BlobSettings>(
    builder.Configuration.GetSection("AzureBlobStorage"));


builder.Services.AddScoped<ITobaccoRepository, TobaccoRepository>();
builder.Services.AddScoped<ITobaccoService, TobaccoService>();
builder.Services.AddScoped<IMasterRepository, MasterRepository>();

builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<IBlobService, BlobService>();


builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services
    .AddValidatorsFromAssemblyContaining<CreateHookahValidator>();

builder.Services
    .AddValidatorsFromAssemblyContaining<EditookahValidator>();

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services
    .AddValidatorsFromAssemblyContaining<CreateTobaccoViewModelValidator>();

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services
    .AddValidatorsFromAssemblyContaining<CreateTobaccoViewModelValidator>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseDeveloperExceptionPage();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();