using Base.Config;
using Base.Entities;
using Base.Models.Requests;
using Base.Repositories;
using Base.Repositories.Interfaces;
using Base.Services;
using Base.Services.Interfaces;
using Infrastructure.Core.Services;
using Infrastructure.Core.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationSetupService.GetConfiguration();

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

app.MapGet("/getOrder/{id}", async (int id, IOrderService orderService) =>
{
    var result = await orderService.GetOrderByIdAsync(id);
    if (result != null)
    {
        return Results.Ok(result);
    }

    return Results.NotFound();
});

app.MapGet("/getOrders", async (int pageIndex, int pageSize, IOrderService orderService) => 
    await orderService.GetOrdersByPageAsync(pageIndex, pageSize));

app.MapDelete("/cancelOrder", async (int id, IOrderService orderService) =>
{
    var result = await orderService.RemoveOrderAsync(id);
    if (result == true)
    {
        return Results.Ok("Success");
    }

    return Results.NotFound();
});

app.MapPost("/addOrder", async ( CreateOrderRequest request, IOrderService orderService) =>
{
    var result = await orderService.CreateOrderAsync(
        request.CustomerId,
        request.OrderStatus,
        request.OrderDate,
        request.RequiredDate,
        request.ShippedDate,
        request.StoreId,
        request.StaffId);
    return result;

});

app.UseCors("CorsPolicy");

app.Run();
