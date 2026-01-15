using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Account.LoginWithRecoveryCode;

public class LoginWithRecoveryCodeEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login-recovery-code", HandleAsync)
            .AddValidationFilter<LoginWithRecoveryCodeRequest>()
            .Produces<LoginWithRecoveryCodeResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Login with Recovery Code")
            .WithDescription("Authenticate a user and return a JWT token using a recovery code.");
    }

    private static async Task<IResult> HandleAsync(
        LoginWithRecoveryCodeRequest request,
        HalcyonDbContext dbContext,
        IHashService hashService,
        IJwtService jwtService,
        CancellationToken cancellationToken = default
    )
    {
        var normalizedEmailAddress = request.EmailAddress.ToLowerInvariant();

        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.NormalizedEmailAddress == normalizedEmailAddress,
            cancellationToken
        );

        if (user is null || user.Password is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        var verified = hashService.VerifyHash(request.Password, user.Password);

        if (!verified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        if (user.IsLockedOut)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "This account has been locked out, please try again later."
            );
        }

        if (!user.IsTwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two-factor authentication is not configured."
            );
        }

        var recoveryCodes = user.TwoFactorRecoveryCodes ?? [];

        string? matchedRecoveryCode = null;

        foreach (var code in recoveryCodes)
        {
            var recoveryCodeVerified = hashService.VerifyHash(request.RecoveryCode, code);

            if (recoveryCodeVerified)
            {
                matchedRecoveryCode = code;
                break;
            }
        }

        if (string.IsNullOrEmpty(matchedRecoveryCode))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid recovery code."
            );
        }

        user.TwoFactorRecoveryCodes = [.. recoveryCodes.Where(code => code != matchedRecoveryCode)];

        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtService.GenerateJwtToken(user);
        return Results.Ok(new LoginWithRecoveryCodeResponse(token));
    }
}
