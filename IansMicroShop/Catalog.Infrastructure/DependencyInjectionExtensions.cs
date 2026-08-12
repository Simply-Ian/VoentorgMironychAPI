namespace IansMicroShop.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterInfrastructureDependencies
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }
}