using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.TwoFactor.RegenerateRecovery;

public record RegenerateRecoveryCodesResponse(Guid UserId, IReadOnlyList<string> RecoveryCodes);

public class RegenerateRecoveryCodesEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/2fa/recovery/regenerate", HandleAsync)
            .RequireAuthorization()
            .Produces<RegenerateRecoveryCodesResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
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

        if (!user.IsTwoFactorEnabled)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two-factor authentication is not enabled."
            );
        }

        var codes = Enumerable
            .Range(0, 8)
            .Select(_ => Guid.NewGuid().ToString("N").Substring(0, 10))
            .ToList();

        user.TwoFactorRecoveryCodes = codes; // In production, store hashed/encrypted
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new RegenerateRecoveryCodesResponse(user.Id, codes));
    }
}
