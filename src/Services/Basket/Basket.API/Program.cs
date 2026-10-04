using System.Reflection;
using Basket.API;
using Basket.API.Infrastructure;
using Carter;
using Common.Middlewares;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddCarter()
    .RegisterMediatR(builder.Configuration)
    .RegisterMarten(builder.Configuration)
    .AddScoped<ICartRepository, MartenCartRepository>()
    .AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));

var app = builder.Build();
app.TurnOnSwagger();
app.UseMiddleware<GracefulExceptionHandling>();
app.MapCarter();
app.Run();