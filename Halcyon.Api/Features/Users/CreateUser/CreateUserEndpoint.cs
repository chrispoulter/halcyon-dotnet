using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Halcyon.Api.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Users.CreateUser;

public class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .AddValidationFilter<CreateUserRequest>()
            .Produces<CreateUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Create User")
            .WithDescription("Create a new user account.");
    }

    private static async Task<IResult> HandleAsync(
        CreateUserRequest request,
        HalcyonDbContext dbContext,
        ISecretHasher secretHasher,
        CancellationToken cancellationToken = default
    )
    {
        var existing = await dbContext.Users.AnyAsync(
            u => u.EmailAddress == request.EmailAddress,
            cancellationToken
        );

        if (existing)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "User name is already taken."
            );
        }

        var user = new User
        {
            EmailAddress = request.EmailAddress,
            Password = secretHasher.GenerateHash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Roles = request.Roles,
        };

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new CreateUserResponse(user.Id));
    }
}
