using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data.Users;

namespace Halcyon.Api.Features.Profile.DeleteProfile;

public class DeleteProfileEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/profile", HandleAsync)
            .RequireAuthorization()
            .Produces<DeleteProfileResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Delete Profile")
            .WithDescription("Delete the profile of the current user.");
    }

    private static async Task<IResult> HandleAsync(
        CurrentUser currentUser,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT 
                id AS Id,
                is_locked_out AS IsLockedOut
            FROM
                users
            WHERE 
                id = @Id
            """,
            new { currentUser.Id }
        );

        if (user is null || user.IsLockedOut)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        await connection.ExecuteAsync(
            """
            DELETE FROM users WHERE id = @Id
            """,
            new { user.Id }
        );

        return Results.Ok(new DeleteProfileResponse(user.Id));
    }
}
