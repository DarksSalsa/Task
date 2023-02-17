using Base.Config;
using Base.Entities;
using Base.Repositories;
using Base.Repositories.Interfaces;
using Base.Services;
using Base.Services.Interfaces;
using Infrastructure.Core.Services;
using Infrastructure.Core.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationSetupService.GetConfiguration();

builder.Services.AddSwaggerGen();
builder.Services.Configure<CoreConfig>(configuration);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddDbContextFactory<BikeStoresContext>(o => o.UseSqlServer(configuration["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<BikeStoresContext>, DbContextWrapper<BikeStoresContext>>();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
    builder => builder
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
