using System.Data;
using Mapster;
using MapsterMapper;
using Promotion.Grpc.Interceptors;
using Promotion.Grpc.Persistence;
using Promotion.Grpc.Mappers;

namespace Promotion.Grpc;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterGrpc(this IServiceCollection services)
    {
        services.AddGrpc(opts =>
        {
            opts.Interceptors.Add<GracefulErrorHadler>();
        });
        services.AddGrpcReflection();
        return services;
    }

    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PromoGrpcService>();
        app.MapGrpcReflectionService();
    }

    public static async Task<WebApplication> SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        try
        {
            await DbExtensions.SeedPromosAsync(connection);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occured while seeding the database");
        }
        return app;
    }
    
    public static IServiceCollection RegisterMediatR(this IServiceCollection services, IConfiguration appConfiguration)
    {
        services.AddMediatR
        (config =>
        {
            config.LicenseKey = appConfiguration.GetSection("LicenseKeys").GetSection("MediatR").Value;
            config.RegisterGenericHandlers = true;
            config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
        });
        return services;
    }
    
    public static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(DependencyInjectionExtensions).Assembly);
        return services;
    }
}