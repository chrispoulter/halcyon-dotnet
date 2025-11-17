using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.DisableTwoFactor;

public class DisableTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/disable-two-factor", HandleAsync)
            .RequireAuthorization()
            .Produces<DisableTwoFactorResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] DisableTwoFactorRequest request,
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

        if (request?.Version is not null && request.Version != user.Version)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Data has been modified since entities were loaded."
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
