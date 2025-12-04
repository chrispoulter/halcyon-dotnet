using System.Reflection;
using Dapper;
using FluentEmail.Core;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data.Users;

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
        IDbConnectionFactory connectionFactory,
        IFluentEmail fluentEmail,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT
                id AS Id,
                email_address AS EmailAddress,
                is_locked_out AS IsLockedOut
            FROM
                users 
            WHERE
                email_address = @Email
            """,
            new { Email = request.EmailAddress }
        );

        if (user is not null && !user.IsLockedOut)
        {
            var token = Guid.NewGuid();

            await connection.ExecuteAsync(
                """
                UPDATE users 
                SET password_reset_token = @Token 
                WHERE id = @Id
                """,
                new { Token = token, user.Id }
            );

            var assembly = Assembly.GetExecutingAssembly();

            await fluentEmail
                .To(user.EmailAddress)
                .Subject("Reset Password // Halcyon")
                .UsingTemplateFromEmbedded(
                    "Halcyon.Api.Features.Account.ForgotPassword.ResetPasswordEmail.html",
                    new { PasswordResetToken = token },
                    assembly
                )
                .SendAsync(cancellationToken);
        }

        return Results.Ok();
    }
}
