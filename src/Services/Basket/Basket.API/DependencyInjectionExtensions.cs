using Basket.API.Models;
using Marten;
using Microsoft.OpenApi;

namespace Basket.API;

public static class DependencyInjectionExtensions
{
    public static WebApplication TurnOnSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
        }

        return app;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc
            (
                "v1",
                new OpenApiInfo
                {
                    Title = "Basket API",
                    Version = "v1"
                }
            );
            config.EnableAnnotations();
            // config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Basket.Domain.xml"));
        });
        return services;
    }

    public static IServiceCollection RegisterMediatR(this IServiceCollection services, IConfiguration appConfiguration)
    {
        services.AddMediatR
        (config =>
        {
            config.LicenseKey = appConfiguration.GetSection("LicenseKeys").GetSection("MediatR").Value;
            config.RegisterGenericHandlers = true;
            // Не регистрирует обобщенные обработчики, потому что ограничение дженерик-параметра
            // where TContract: class слишком широкое (Камянецкий)
            config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
        });
        return services;
    }

    public static IServiceCollection RegisterMarten(this IServiceCollection services, IConfiguration appConfiguration)
    {
        services.AddMarten(opts =>
        {
            opts.Connection(appConfiguration.GetConnectionString("BasketDb")!);
            opts.Schema.For<ShoppingCart>().Identity(x => x.AccountName);
        }).UseLightweightSessions();
        return services;
    }
}