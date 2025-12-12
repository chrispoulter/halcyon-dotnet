using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Profile.SetupTwoFactor;

public class SetupTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/profile/setup-two-factor", HandleAsync)
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

        if (string.IsNullOrEmpty(user.TwoFactorTempSecret))
        {
            var rawKey = KeyGeneration.GenerateRandomKey(20);
            var secret = Base32Encoding.ToString(rawKey);

            user.TwoFactorTempSecret = secret;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var issuer = configuration["TwoFactor:Issuer"] ?? "Halcyon";
        var label = $"{issuer}:{user.EmailAddress}";
        var otpauthUri =
            $"otpauth://totp/{Uri.EscapeDataString(label)}?secret={user.TwoFactorTempSecret}&issuer={Uri.EscapeDataString(issuer)}&digits=6&period=30";

        return Results.Ok(
            new SetupTwoFactorResponse(user.Id, otpauthUri, user.TwoFactorTempSecret)
        );
    }
}
