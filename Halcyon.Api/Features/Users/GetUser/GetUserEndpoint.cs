using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data.Users;

namespace Halcyon.Api.Features.Users.GetUser;

public class GetUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<GetUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Get User")
            .WithDescription("Retrieve a user account by ID.");
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT 
                id AS Id,
                email_address AS EmailAddress,
                first_name AS FirstName,
                last_name AS LastName,
                date_of_birth AS DateOfBirth,
                is_locked_out AS IsLockedOut,
                roles AS Roles
            FROM 
                users
            WHERE 
                id = @Id
            """,
            new { Id = id }
        );

        if (user is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        var result = new GetUserResponse(
            user.Id,
            user.EmailAddress,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.IsLockedOut,
            user.Roles
        );

        return Results.Ok(result);
    }
}
