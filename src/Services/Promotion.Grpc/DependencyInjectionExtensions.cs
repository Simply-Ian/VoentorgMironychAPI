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