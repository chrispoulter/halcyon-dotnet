using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Profile.ChangePassword;

public class ChangePasswordEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile/change-password", HandleAsync)
            .RequireAuthorization()
            .AddValidationFilter<ChangePasswordRequest>()
            .Produces<ChangePasswordResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Change Password")
            .WithDescription("Change the password for the current user.");
    }

    private static async Task<IResult> HandleAsync(
        ChangePasswordRequest request,
        CurrentUser currentUser,
        NpgsqlDataSource dataSource,
        IPasswordHasher passwordHasher
    )
    {
        using var connection = dataSource.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT id AS Id, password AS Password, is_locked_out AS IsLockedOut
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

        if (user.Password is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Incorrect password."
            );
        }

        var verified = passwordHasher.VerifyPassword(request.CurrentPassword, user.Password);

        if (!verified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Incorrect password."
            );
        }

        var password = passwordHasher.HashPassword(request.NewPassword);

        await connection.ExecuteAsync(
            """
            UPDATE users
            SET password = @Password, password_reset_token = NULL
            WHERE id = @Id
            """,
            new { Password = password, user.Id }
        );

        return Results.Ok(new ChangePasswordResponse(user.Id));
    }
}
