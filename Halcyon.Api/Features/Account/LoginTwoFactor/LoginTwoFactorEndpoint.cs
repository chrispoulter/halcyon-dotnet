using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Account.LoginTwoFactor;

public class LoginTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login/2fa", HandleAsync)
            .AddValidationFilter<LoginTwoFactorRequest>()
            .Produces<LoginTwoFactorResponse>()
            .WithTags(Tags.Account);
    }

    private static async Task<IResult> HandleAsync(
        LoginTwoFactorRequest request,
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

        var verified = passwordHasher.VerifyPassword(request.Password, user.Password);

        if (!verified)
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

        if (!string.IsNullOrEmpty(request.RecoveryCode))
        {
            var input = request.RecoveryCode;
            var codes = user.TwoFactorRecoveryCodes ?? [];
            var matched = codes.FirstOrDefault(code => code == input);

            if (matched is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid recovery code."
                );
            }

            user.TwoFactorRecoveryCodes = [.. codes.Where(code => code != input)];

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else if (!string.IsNullOrEmpty(request.Code))
        {
            var totp = new Totp(
                Base32Encoding.ToBytes(user.TwoFactorSecret),
                step: 30,
                totpSize: 6
            );

            var totpVerified = totp.VerifyTotp(
                request.Code,
                out _,
                VerificationWindow.RfcSpecifiedNetworkDelay
            );

            if (!totpVerified)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid code."
                );
            }
        }

        var token = jwtTokenGenerator.GenerateJwtToken(user);
        return Results.Ok(new LoginTwoFactorResponse(token));
    }
}
