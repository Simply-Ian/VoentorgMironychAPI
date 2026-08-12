namespace IansMicroShop.Catalog.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterApplicationDependencies
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }
}