using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Users.GetUser;

public class GetUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .Produces<GetUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Get User")
            .WithDescription("Retrieve a user account by ID.");
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        HalcyonDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        var result = new GetUserResponse(
            user.Id,
            user.EmailAddress,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.IsTwoFactorEnabled,
            user.IsLockedOut,
            user.Roles
        );

        return Results.Ok(result);
    }
}
