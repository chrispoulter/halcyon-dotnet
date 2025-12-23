using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Profile.UpdateProfile;

public class UpdateProfileEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile", HandleAsync)
            .RequireAuthorization()
            .AddValidationFilter<UpdateProfileRequest>()
            .Produces<UpdateProfileResponse>()
            .WithTags(Tags.Profile)
            .WithSummary("Update Profile")
            .WithDescription("Update the profile of the current user.");
    }

    private static async Task<IResult> HandleAsync(
        UpdateProfileRequest request,
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

        var normalizedEmailAddress = request.EmailAddress.ToLowerInvariant();

        if (!normalizedEmailAddress.Equals(user.NormalizedEmailAddress))
        {
            var existing = await dbContext.Users.AnyAsync(
                u => u.NormalizedEmailAddress == normalizedEmailAddress,
                cancellationToken
            );

            if (existing)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "User name is already taken."
                );
            }
        }

        user.EmailAddress = request.EmailAddress;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.DateOfBirth = request.DateOfBirth;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new UpdateProfileResponse(user.Id));
    }
}
