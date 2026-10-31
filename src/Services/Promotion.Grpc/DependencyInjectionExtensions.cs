using System.Data;
using Promotion.Grpc.Persistence;

namespace Promotion.Grpc;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterGrpc(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        return services;
    }

    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGrpcService<GreeterService>();
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
}