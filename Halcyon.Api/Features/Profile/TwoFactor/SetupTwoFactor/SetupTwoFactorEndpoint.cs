using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Halcyon.Api.Data.Users;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Profile.TwoFactor.SetupTwoFactor;

public class SetupTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/2fa/setup", HandleAsync)
            .RequireAuthorization()
            .Produces<SetupTwoFactorResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        CurrentUser currentUser,
        HalcyonDbContext dbContext,
        IConfiguration configuration,
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

        // Generate a new temporary secret each time setup is called.
        var rawKey = KeyGeneration.GenerateRandomKey(20);
        var secret = Base32Encoding.ToString(rawKey); // Base32 for common authenticator apps

        var issuer = configuration["TwoFactor:Issuer"] ?? "Halcyon";
        var label = $"{issuer}:{user.EmailAddress}";
        var otpauthUri =
            $"otpauth://totp/{Uri.EscapeDataString(label)}?secret={secret}&issuer={Uri.EscapeDataString(issuer)}&digits=6&period=30";

        // Persist temporary secret (not yet enabled)
        user.TwoFactorTempSecret = secret;
        user.IsTwoFactorEnabled = false; // ensure disabled until verified
        await dbContext.SaveChangesAsync(cancellationToken);

        // QrContent is the URI itself (client can render a QR code from it)
        var response = new SetupTwoFactorResponse(secret, otpauthUri, otpauthUri);
        return Results.Ok(response);
    }
}
