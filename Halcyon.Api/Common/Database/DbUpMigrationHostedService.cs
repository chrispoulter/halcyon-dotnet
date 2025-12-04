using System.Reflection;
using DbUp;

namespace Halcyon.Api.Common.Database;

public class DbUpMigrationHostedService : IHostedService
{
    private readonly ILogger<DbUpMigrationHostedService> _logger;
    private readonly IConfiguration _configuration;

    public DbUpMigrationHostedService(
        ILogger<DbUpMigrationHostedService> logger,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("Database");

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
            _logger.LogError(result.Error, "DbUp migration failed");
            throw result.Error;
        }

        _logger.LogInformation("DbUp migration completed successfully.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
