using System.Reflection;
using Dapper;
using FluentEmail.Core;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Npgsql;

namespace Halcyon.Api.Features.Account.ForgotPassword;

public class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/account/forgot-password", HandleAsync)
            .AddValidationFilter<ForgotPasswordRequest>()
            .WithTags(Tags.Account)
            .WithSummary("Forgot Password")
            .WithDescription("Initiate the password reset process for a user.");
    }

    private static async Task<IResult> HandleAsync(
        ForgotPasswordRequest request,
        NpgsqlDataSource dataSource,
        IFluentEmail fluentEmail,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = dataSource.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT id AS Id, email_address AS EmailAddress, is_locked_out AS IsLockedOut
            FROM users 
            WHERE email_address = @Email
            """,
            new { Email = request.EmailAddress }
        );

        if (user is not null && !user.IsLockedOut)
        {
            var passwordResetToken = Guid.NewGuid();

            await connection.ExecuteAsync(
                """
                UPDATE users 
                SET password_reset_token = @Token 
                WHERE id = @Id
                """,
                new { Token = passwordResetToken, user.Id }
            );

            var assembly = Assembly.GetExecutingAssembly();

            await fluentEmail
                .To(user.EmailAddress)
                .Subject("Reset Password // Halcyon")
                .UsingTemplateFromEmbedded(
                    "Halcyon.Api.Features.Account.ForgotPassword.ResetPasswordEmail.html",
                    new { PasswordResetToken = passwordResetToken },
                    assembly
                )
                .SendAsync(cancellationToken);
        }

        return Results.Ok();
    }
}
