using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Npgsql;

namespace Halcyon.Api.Features.Users.CreateUser;

public class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .AddValidationFilter<CreateUserRequest>()
            .Produces<CreateUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Create User")
            .WithDescription("Create a new user account.");
    }

    private static async Task<IResult> HandleAsync(
        CreateUserRequest request,
        NpgsqlDataSource dataSource,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = dataSource.CreateConnection();

        var existing = await connection.ExecuteScalarAsync<bool>(
            """
            SELECT EXISTS(
                SELECT 1 FROM users WHERE email_address = @Email
            )
            """,
            new { Email = request.EmailAddress }
        );

        if (existing)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "User name is already taken."
            );
        }

        var password = passwordHasher.HashPassword(request.Password);

        var userId = await connection.ExecuteScalarAsync<Guid>(
            """
            INSERT INTO users (email_address, password, first_name, last_name, date_of_birth, roles) 
            VALUES (@EmailAddress, @Password, @FirstName, @LastName, @DateOfBirth, @Roles)
            RETURNING id;
            """,
            new
            {
                request.EmailAddress,
                Password = password,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Roles,
            }
        );

        return Results.Ok(new CreateUserResponse(userId));
    }
}
