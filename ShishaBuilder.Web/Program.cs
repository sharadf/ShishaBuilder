using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Business.Repositories;
using ShishaBuilder.Business.Repositories.MasterRepositories;
using ShishaBuilder.Business.Services.BlobServices;
using ShishaBuilder.Business.Services.MasterServices;
using ShishaBuilder.Business.Services.TobaccoServices;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Repositories;
using ShishaBuilder.Core.Repositories.MasterRepositories;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.TobaccoServices;
using ShishaBuilder.Core.Settings;
using ShishaBuilder.Core.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options=>{
    var coonectionString= builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(coonectionString);
});

builder.Services.Configure<BlobSettings>(
    builder.Configuration.GetSection("AzureBlobStorage"));


builder.Services.AddScoped<ITobaccoRepository,TobaccoRepository>();
builder.Services.AddScoped<ITobaccoService,TobaccoService>();
builder.Services.AddScoped<IMasterRepository,MasterRepository>();

builder.Services.AddScoped<IMasterService,MasterService>();
builder.Services.AddScoped<IBlobService,BlobService>();


builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services
    .AddValidatorsFromAssemblyContaining<CreateTobaccoViewModelValidator>();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
// ToDo 
//     Dobavit knopku deleted masters ,
//     Dobavit Nomer telefona k modeli master,
//     Dobavit Validatori,
//     Sdelat tak ctob v edit master on ne mog izmenyat pole Name , Telefon Num
//     Dobavit emu list zakazov (esli na tot moment sharaf zakoncit svoyu rabotu)
