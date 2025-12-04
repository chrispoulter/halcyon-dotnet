using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;

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
        IDbConnectionFactory connectionFactory,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

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

        var id = Guid.NewGuid();
        var password = passwordHasher.HashPassword(request.Password);

        await connection.ExecuteAsync(
            """
            INSERT INTO users (
                id, email_address, password, first_name, last_name, date_of_birth, roles, is_locked_out
            ) VALUES (
                @Id, @EmailAddress, @Password, @FirstName, @LastName, @DateOfBirth, @Roles, FALSE
            )
            """,
            new
            {
                Id = id,
                request.EmailAddress,
                Password = password,
                request.FirstName,
                request.LastName,
                DateOfBirth = request.DateOfBirth.ToString("YYYY-MM-DD"),
                request.Roles,
            }
        );

        return Results.Ok(new CreateUserResponse(id));
    }
}
