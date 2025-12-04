using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Users.DeleteUser;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<DeleteUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Delete User")
            .WithDescription("Delete a user account by ID.");
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

        if (user.Id == currentUser.Id)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot delete currently logged in user."
            );
        }

        await connection.ExecuteAsync(
            """
            DELETE FROM users 
            WHERE id = @Id
            """,
            new { Id = id }
        );

        return Results.Ok(new DeleteUserResponse(user.Id));
    }
}
