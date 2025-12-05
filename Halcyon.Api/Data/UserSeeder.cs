using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Halcyon.Api.Data;

public class UserSeeder(
    NpgsqlDataSource dataSource,
    IPasswordHasher passwordHasher,
    IOptions<SeedSettings> seedSettings
) : IDbSeeder
{
    private readonly SeedSettings _seedSettings = seedSettings.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var connection = dataSource.CreateConnection();

        foreach (var seedUser in _seedSettings.Users)
        {
            var password = passwordHasher.HashPassword(seedUser.Password);

            await connection.ExecuteAsync(
                """
                INSERT INTO users (
                    email_address, 
                    password, 
                    password_reset_token, 
                    first_name, 
                    last_name,
                    date_of_birth, 
                    is_locked_out, 
                    roles
                )
                VALUES (
                    @EmailAddress, 
                    @Password, 
                    NULL, 
                    @FirstName, 
                    @LastName, 
                    @DateOfBirth, 
                    false, 
                    @Roles
                ) 
                ON CONFLICT (email_address) 
                DO UPDATE SET 
                    email_address = EXCLUDED.email_address, 
                    password = EXCLUDED.password, 
                    password_reset_token = EXCLUDED.password_reset_token, 
                    first_name = EXCLUDED.first_name, 
                    last_name = EXCLUDED.last_name, 
                    date_of_birth = EXCLUDED.date_of_birth, 
                    is_locked_out = EXCLUDED.is_locked_out, 
                    roles = EXCLUDED.roles
                """,
                new
                {
                    seedUser.EmailAddress,
                    Password = password,
                    seedUser.FirstName,
                    seedUser.LastName,
                    seedUser.DateOfBirth,
                    seedUser.Roles,
                }
            );
        }
    }
}
