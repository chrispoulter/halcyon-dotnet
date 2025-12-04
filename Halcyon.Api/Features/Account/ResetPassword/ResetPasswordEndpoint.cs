using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;

namespace Halcyon.Api.Features.Account.ResetPassword;

public class ResetPasswordEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/account/reset-password", HandleAsync)
            .AddValidationFilter<ResetPasswordRequest>()
            .Produces<ResetPasswordResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Reset Password")
            .WithDescription("Reset a user's password using a valid token.");
    }

    private static async Task<IResult> HandleAsync(
        ResetPasswordRequest request,
        IDbConnectionFactory connectionFactory,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT id AS Id, password_reset_token AS PasswordResetToken, is_locked_out AS IsLockedOut
            FROM users WHERE email_address = @Email
            """,
            new { Email = request.EmailAddress }
        );

        if (user is null || user.IsLockedOut || request.Token != user.PasswordResetToken)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid token."
            );
        }

        var newHash = passwordHasher.HashPassword(request.NewPassword);

        await connection.ExecuteAsync(
            """
            UPDATE users
            SET password = @Password,
                password_reset_token = NULL
            WHERE id = @Id
            """,
            new { Password = newHash, user.Id }
        );

        return Results.Ok(new ResetPasswordResponse(user!.Id));
    }
}
