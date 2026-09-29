using Basket.API;
using Carter;
using Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddCarter()
    .RegisterMediatR(builder.Configuration);

var app = builder.Build();
app.TurnOnSwagger();
app.UseMiddleware<GracefulExceptionHandling>();
app.MapCarter();
app.Run();