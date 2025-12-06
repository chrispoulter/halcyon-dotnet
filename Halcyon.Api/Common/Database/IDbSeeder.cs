namespace Halcyon.Api.Common.Database;

public interface IDbSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
