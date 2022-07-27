namespace AmazonSimpleEmail.API.Configuration;

public static class DistributedCacheConfig
{
    public static IServiceCollection AddDistributedCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();

        return services;
    }
}
