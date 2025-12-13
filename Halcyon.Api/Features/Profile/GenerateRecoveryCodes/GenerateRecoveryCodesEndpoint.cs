using System.Security.Cryptography;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.GenerateRecoveryCodes;

public class GenerateRecoveryCodesEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile/generate-recovery-codes", HandleAsync)
            .RequireAuthorization()
            .Produces<GenerateRecoveryCodesResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Generate Recovery Codes")
            .WithDescription("Generate new recovery codes for the current user.");
    }

    private static async Task<IResult> HandleAsync(
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

        if (!user.IsTwoFactorEnabled)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two-factor authentication is not configured."
            );
        }

        var recoveryCodes = Enumerable
            .Range(0, 10)
            .Select(_ => Convert.ToHexString(RandomNumberGenerator.GetBytes(5)).ToUpperInvariant())
            .ToList();

        var hashedRecoveryCodes = recoveryCodes.Select(passwordHasher.HashPassword).ToList();

        user.TwoFactorRecoveryCodes = hashedRecoveryCodes;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new GenerateRecoveryCodesResponse(user.Id, recoveryCodes));
    }
}
