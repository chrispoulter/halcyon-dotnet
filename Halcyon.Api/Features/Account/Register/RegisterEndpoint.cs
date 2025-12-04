using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;

namespace Halcyon.Api.Features.Account.Register;

public class RegisterEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/register", HandleAsync)
            .AddValidationFilter<RegisterRequest>()
            .Produces<RegisterResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Register")
            .WithDescription("Register a new user account.");
    }

    private static async Task<IResult> HandleAsync(
        RegisterRequest request,
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
        var passwordHash = passwordHasher.HashPassword(request.Password);

        await connection.ExecuteAsync(
            """
            INSERT INTO users (
                id,
                email_address,
                password,
                first_name,
                last_name,
                date_of_birth,
                is_locked_out
            ) 
            VALUES (
                @Id,
                @EmailAddress,
                @Password,
                @FirstName,
                @LastName,
                @DateOfBirth,
                FALSE
            )
            """,
            new
            {
                Id = id,
                request.EmailAddress,
                Password = passwordHash,
                request.FirstName,
                request.LastName,
                DateOfBirth = request.DateOfBirth.ToString("YYYY-MM-DD"),
            }
        );

        return Results.Ok(new RegisterResponse(id));
    }
}
