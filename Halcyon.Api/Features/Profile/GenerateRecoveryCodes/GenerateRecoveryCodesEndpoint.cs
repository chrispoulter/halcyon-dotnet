using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.GenerateRecoveryCodes;

public class GenerateRecoveryCodesEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/generate-recovery-codes", HandleAsync)
            .RequireAuthorization()
            .Produces<GenerateRecoveryCodesResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] GenerateRecoveryCodesRequest request,
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

        if (!user.IsTwoFactorEnabled)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Two-factor authentication is not enabled."
            );
        }

        var codes = Enumerable.Range(0, 8).Select(_ => Guid.NewGuid().ToString("N")).ToList();
        user.TwoFactorRecoveryCodes = codes;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new GenerateRecoveryCodesResponse(user.Id, codes));
    }
}
