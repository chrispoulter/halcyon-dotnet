using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Profile.VerifyTwoFactor;

public class VerifyTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile/verify-two-factor", HandleAsync)
            .RequireAuthorization()
            .Produces<VerifyTwoFactorResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        VerifyTwoFactorRequest request,
        CurrentUser currentUser,
        HalcyonDbContext dbContext,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == currentUser.Id,
            cancellationToken
        );

        if (user is null || user.IsLockedOut)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        if (string.IsNullOrEmpty(user.TwoFactorTempSecret))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two factor authentication configuration not found."
            );
        }

        var totp = new Totp(
            Base32Encoding.ToBytes(user.TwoFactorTempSecret),
            step: 30,
            totpSize: 6
        );

        var verified = totp.VerifyTotp(
            request.Code,
            out _,
            VerificationWindow.RfcSpecifiedNetworkDelay
        );

        if (!verified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid authenticator code."
            );
        }

        var recoveryCodes = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid().ToString("N"));
        var hashedRecoveryCodes = recoveryCodes.Select(passwordHasher.HashPassword);

        user.IsTwoFactorEnabled = true;
        user.TwoFactorSecret = user.TwoFactorTempSecret;
        user.TwoFactorTempSecret = null;
        user.TwoFactorRecoveryCodes = recoveryCodes;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new VerifyTwoFactorResponse(user.Id, recoveryCodes));
    }
}
