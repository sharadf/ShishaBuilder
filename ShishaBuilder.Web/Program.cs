using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Business.Data;
using ShishaBuilder.Business.Repositories;
using ShishaBuilder.Core.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IHookahRepository, HookahRepository>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<IHookahService, HookahService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseRouting();

app.UseDeveloperExceptionPage(); 

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
