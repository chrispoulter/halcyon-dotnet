using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Users.LockUser;

public class LockUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}/lock", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<LockUserResponse>()
            .WithTags(Tags.Users);
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        CurrentUser currentUser,
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

        if (user.Id == currentUser.Id)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot lock currently logged in user."
            );
        }

        user.IsLockedOut = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new LockUserResponse(user.Id));
    }
}
