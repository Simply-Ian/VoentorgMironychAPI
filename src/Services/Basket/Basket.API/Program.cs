using Basket.API;
using Basket.API.Infrastructure;
using Carter;
using Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddCarter()
    .RegisterMediatR(builder.Configuration)
    .RegisterMarten(builder.Configuration)
    .AddScoped<ICartRepository, MartenCartRepository>();

var app = builder.Build();
app.TurnOnSwagger();
app.UseMiddleware<GracefulExceptionHandling>();
app.MapCarter();
app.Run();