using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Profile.TwoFactor.VerifyTwoFactor;

public class VerifyTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/2fa/verify", HandleAsync)
            .RequireAuthorization()
            .Produces<VerifyTwoFactorResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        VerifyTwoFactorRequest request,
        CurrentUser currentUser,
        HalcyonDbContext dbContext,
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
                title: "No 2FA setup in progress."
            );
        }

        if (string.IsNullOrEmpty(request.Code))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Code is required."
            );
        }

        var totp = new Totp(
            Base32Encoding.ToBytes(user.TwoFactorTempSecret),
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

        user.TwoFactorSecret = user.TwoFactorTempSecret;
        user.TwoFactorTempSecret = null;
        user.IsTwoFactorEnabled = true;

        var codes = Enumerable.Range(0, 8).Select(_ => Guid.NewGuid().ToString("N")[..10]).ToList();

        user.TwoFactorRecoveryCodes = codes; // TODO: hash or encrypt in production

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new VerifyTwoFactorResponse(user.Id, user.IsTwoFactorEnabled, codes));
    }
}
