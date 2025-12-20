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
        var emailAddresses = _seedSettings.Users.Select(u => u.EmailAddress);

        var users = await dbContext
            .Users.Where(u => emailAddresses.Contains(u.EmailAddress))
            .ToListAsync(cancellationToken);

        foreach (var seedUser in _seedSettings.Users)
        {
            var user = users.FirstOrDefault(u => u.EmailAddress == seedUser.EmailAddress);

            if (user is null)
            {
                user = new();
                dbContext.Users.Add(user);
            }

            user.EmailAddress = seedUser.EmailAddress;
            user.Password = secretHasher.GenerateHash(seedUser.Password);
            user.PasswordResetToken = null;
            user.IsTwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            user.TwoFactorRecoveryCodes = null;
            user.FirstName = seedUser.FirstName;
            user.LastName = seedUser.LastName;
            user.DateOfBirth = seedUser.DateOfBirth;
            user.IsLockedOut = false;
            user.Roles = seedUser.Roles;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
