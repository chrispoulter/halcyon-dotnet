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
            .WithTags(Tags.Profile);
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
                title: "Two factor authentication is not enabled."
            );
        }

        var recoveryCodes = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid().ToString("N"));
        var hashedRecoveryCodes = recoveryCodes.Select(passwordHasher.HashPassword);

        user.TwoFactorRecoveryCodes = hashedRecoveryCodes;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new GenerateRecoveryCodesResponse(user.Id, recoveryCodes));
    }
}
