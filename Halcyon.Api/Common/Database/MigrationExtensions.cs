namespace Halcyon.Api.Common.Database;

public static class MigrationExtensions
{
    public static IServiceCollection AddMigration(this IServiceCollection services)
    {
        services.AddHostedService<MigrationBackgroundService>();

        return services;
    }

    public static IServiceCollection AddSeeder<TDbSeeder>(this IServiceCollection services)
        where TDbSeeder : class, IDbSeeder
    {
        services.AddScoped<IDbSeeder, TDbSeeder>();

        return services;
    }
}
