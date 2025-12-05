using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Users.LockUser;

public class LockUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}/lock", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<LockUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Lock User")
            .WithDescription("Lock a user account by ID.");
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        CurrentUser currentUser,
        NpgsqlDataSource dataSource,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = dataSource.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT id AS Id
            FROM users
            WHERE id = @Id
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

        if (user.Id == currentUser.Id)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot lock currently logged in user."
            );
        }

        await connection.ExecuteAsync(
            """
            UPDATE users 
            SET is_locked_out = TRUE 
            WHERE id = @Id
            """,
            new { user.Id }
        );

        return Results.Ok(new LockUserResponse(user.Id));
    }
}
