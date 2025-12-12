using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Account.LoginWithTwoFactor;

public class LoginWithTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login-two-factor", HandleAsync)
            .AddValidationFilter<LoginWithTwoFactorRequest>()
            .Produces<LoginWithTwoFactorResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Login with Two-Factor")
            .WithDescription("Authenticate a user and return a JWT token using an authenticator code."); ;
    }

    private static async Task<IResult> HandleAsync(
        LoginWithTwoFactorRequest request,
        HalcyonDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress, cancellationToken);

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

        var totp = new Totp(Base32Encoding.ToBytes(user.TwoFactorSecret), step: 30, totpSize: 6);

        var totpVerified = totp.VerifyTotp(
            request.Code,
            out _,
            VerificationWindow.RfcSpecifiedNetworkDelay
        );

        if (!totpVerified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid authenticator code."
            );
        }

        var token = jwtTokenGenerator.GenerateJwtToken(user);
        return Results.Ok(new LoginWithTwoFactorResponse(token));
    }
}
