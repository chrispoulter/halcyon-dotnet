using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Users.UnlockUser;

public class UnlockUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}/unlock", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<UnlockUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Unlock User")
            .WithDescription("Unlock a user account by ID.");
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        HalcyonDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        user.IsLockedOut = false;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new UnlockUserResponse(user.Id));
    }
}
