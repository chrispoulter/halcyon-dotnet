using System.Reflection;
using DbUp;

namespace Halcyon.Api.Common.Database;

public class MigrationBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<MigrationBackgroundService> logger,
    IConfiguration configuration
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Migrating database");
        }

        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Database' is not configured.");
        }

        var upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(result.Error, "An error occurred while migrating database");
            }

            return;
        }

        using var scope = serviceProvider.CreateScope();
        var dbSeeders = scope.ServiceProvider.GetServices<IDbSeeder>();

        if (!dbSeeders.Any())
        {
            return;
        }

        foreach (var seeder in dbSeeders)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Seeding database with {DbSeeder}", seeder.GetType().Name);
            }

            try
            {
                await seeder.SeedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(
                        ex,
                        "An error occurred while seeding database with {DbSeeder}",
                        seeder.GetType().Name
                    );
                }
            }
        }
    }
}
