
using MagicVilla_API;
using MagicVilla_API.Data;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    
});
builder.Services.AddScoped<IVillaRepository,VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository,VillaNumberRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Add services to the container.
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/VillaLogs.txt", rollingInterval:RollingInterval.Day)
//    .CreateLogger();
//builder.Host.UseSerilog();
builder.Services.AddControllers(Options =>
{
    //Options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
//ghi nhật ký
//builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseEndpoints(endpoint =>{
//     endpoint.MapControllerRoute(
//         name: default,
//         pattern: {controller = VillaAPI}/{Action = }
//     )
// })
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
