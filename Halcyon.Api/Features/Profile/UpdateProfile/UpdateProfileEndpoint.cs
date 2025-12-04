using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Profile.UpdateProfile;

public class UpdateProfileEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile", HandleAsync)
            .RequireAuthorization()
            .AddValidationFilter<UpdateProfileRequest>()
            .Produces<UpdateProfileResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Update Profile")
            .WithDescription("Update the profile of the current user.");
    }

    private static async Task<IResult> HandleAsync(
        UpdateProfileRequest request,
        CurrentUser currentUser,
        NpgsqlDataSource dataSource,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = dataSource.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT 
                id AS Id,
                email_address AS EmailAddress,
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

        if (
            !request.EmailAddress.Equals(
                user.EmailAddress,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
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
        }

        var rows = await connection.ExecuteAsync(
            """
            UPDATE users
            SET email_address = @EmailAddress,
                first_name = @FirstName,
                last_name = @LastName,
                date_of_birth = @DateOfBirth
            WHERE id = @Id
            """,
            new
            {
                request.EmailAddress,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                user.Id,
            }
        );

        return Results.Ok(new UpdateProfileResponse(user.Id));
    }
}
