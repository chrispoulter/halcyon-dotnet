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
            .WithTags(Tags.Account);
    }

    private static async Task<IResult> HandleAsync(
        LoginWithRecoveryCodeRequest request,
        HalcyonDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.EmailAddress == request.EmailAddress,
            cancellationToken
        );

        if (user is null || user.Password is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid credentials."
            );
        }

        var passwordVerified = passwordHasher.VerifyPassword(request.Password, user.Password);

        if (!passwordVerified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid credentials."
            );
        }

        if (!user.IsTwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two-factor authentication is not enabled."
            );
        }

        var recoveryCodes = user.TwoFactorRecoveryCodes ?? [];

        var matchedRecoveryCode = recoveryCodes.Any(code => code == request.RecoveryCode);

        if (!matchedRecoveryCode)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid recovery code."
            );
        }

        user.TwoFactorRecoveryCodes =
        [
            .. recoveryCodes.Where(code => code != request.RecoveryCode),
        ];

        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtTokenGenerator.GenerateJwtToken(user);
        return Results.Ok(new LoginWithRecoveryCodeResponse(token));
    }
}
