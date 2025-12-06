using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Account.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login", HandleAsync)
            .AddValidationFilter<LoginRequest>()
            .Produces<LoginResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Login")
            .WithDescription("Authenticate a user and return a JWT token.");
    }

    private static async Task<IResult> HandleAsync(
        LoginRequest request,
        NpgsqlDataSource dataSource,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = dataSource.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT
                id AS Id,
                email_address AS EmailAddress,
                first_name AS FirstName,
                last_name AS LastName,
                password AS Password,
                roles AS Roles
            FROM users
            WHERE email_address = @Email
            """,
            new { Email = request.EmailAddress }
        );

        if (user is null || user.Password is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        var verified = passwordHasher.VerifyPassword(request.Password, user.Password);

        if (!verified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        if (user.IsLockedOut)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "This account has been locked out, please try again later."
            );
        }

        var token = jwtTokenGenerator.GenerateJwtToken(user);
        var result = new LoginResponse(token);

        return Results.Ok(result);
    }
}
