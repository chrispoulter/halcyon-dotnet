using System.Data;
using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data.Users;

namespace Halcyon.Api.Features.Users.UnlockUser;

public class UnlockUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}/unlock", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<UnlockUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Unlock User")
            .WithDescription("Unlock a user account by ID.");
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
                id AS Id
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

        await connection.ExecuteAsync(
            """
            UPDATE users 
            SET is_locked_out = FALSE 
            WHERE id = @Id
            """,
            new { user.Id }
        );

        return Results.Ok(new UnlockUserResponse(id));
    }
}
