using Catalog.Application.Behaviors;
using Catalog.Application.Handlers;

namespace Catalog.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterApplicationDependencies
    (
        this IServiceCollection services,
        IConfiguration appConfiguration
    )
    {
        services.AddMediatR
        (
            config =>
            {
                config.LicenseKey = appConfiguration.GetSection("LicenseKeys").GetSection("MediatR").Value;
                config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ExceptionActionProcessorBehavior<,>));
            }
        );
        return services;
    }
}