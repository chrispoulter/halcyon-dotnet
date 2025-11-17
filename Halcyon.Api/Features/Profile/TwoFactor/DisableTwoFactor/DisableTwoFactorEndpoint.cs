using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.TwoFactor.DisableTwoFactor;

public class DisableTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/2fa/disable", HandleAsync)
            .RequireAuthorization()
            .Produces<DisableTwoFactorResponse>()
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

        user.IsTwoFactorEnabled = false;
        user.TwoFactorSecret = null;
        user.TwoFactorTempSecret = null;
        user.TwoFactorRecoveryCodes = null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new DisableTwoFactorResponse(user.Id));
    }
}
