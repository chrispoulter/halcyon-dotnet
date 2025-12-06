using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Profile.GetProfile;

public class GetProfileEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/profile", HandleAsync)
            .RequireAuthorization()
            .Produces<GetProfileResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Get Profile")
            .WithDescription("Retrieve the profile of the current user.");
    }

    private static async Task<IResult> HandleAsync(
        CurrentUser currentUser,
        NpgsqlDataSource dataSource,
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
                date_of_birth AS DateOfBirth,
                is_locked_out AS IsLockedOut
            FROM users
            WHERE id = @Id
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

        var result = new GetProfileResponse(
            user.Id,
            user.EmailAddress,
            user.FirstName,
            user.LastName,
            user.DateOfBirth
        );

        return Results.Ok(result);
    }
}
