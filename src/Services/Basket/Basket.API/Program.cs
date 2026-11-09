using System.Reflection;
using Basket.API;
using Basket.API.Infrastructure;
using Carter;
using Common.Middlewares;
using FluentValidation;
using Promotion.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddCarter()
    .RegisterMediatR(builder.Configuration)
    .RegisterMarten(builder.Configuration)
    .AddScoped<ICartRepository, MartenCartRepository>()
    .RegisterRedisCache(builder.Configuration)
    .AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)))
    .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddGrpcClient<PromoService.PromoServiceClient>(
    opts =>
    {
        opts.Address = new Uri(
            builder.Configuration.GetSection("Grpc:PromoServiceUrl").Value!
        );
    }
);

var app = builder.Build();
app.TurnOnSwagger();
app.UseExceptionHandler(_ => { });
app.MapCarter();
app.Run();