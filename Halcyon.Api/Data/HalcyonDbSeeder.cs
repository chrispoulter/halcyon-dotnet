using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Halcyon.Api.Data;

public class HalcyonDbSeeder(
    HalcyonDbContext dbContext,
    ISecretHasher secretHasher,
    IOptions<SeedSettings> seedSettings
) : IDbSeeder<HalcyonDbContext>
{
    private readonly SeedSettings _seedSettings = seedSettings.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var normalizedEmailAddresses = _seedSettings.Users.Select(u =>
            u.EmailAddress.ToLowerInvariant()
        );

        var users = await dbContext
            .Users.Where(u => normalizedEmailAddresses.Contains(u.NormalizedEmailAddress))
            .ToListAsync(cancellationToken);

        foreach (var seedUser in _seedSettings.Users)
        {
            var normalizedEmailAddress = seedUser.EmailAddress.ToLowerInvariant();

            var user = users.FirstOrDefault(u =>
                u.NormalizedEmailAddress == normalizedEmailAddress
            );

            if (user is null)
            {
                user = new();
                dbContext.Users.Add(user);
            }

            user.EmailAddress = seedUser.EmailAddress;
            user.Password = secretHasher.GenerateHash(seedUser.Password);
            user.FirstName = seedUser.FirstName;
            user.LastName = seedUser.LastName;
            user.DateOfBirth = seedUser.DateOfBirth;
            user.Roles = seedUser.Roles;
            user.IsLockedOut = false;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
