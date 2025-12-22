using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Account.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login", HandleAsync)
            .AddValidationFilter<LoginRequest>()
            .Produces<LoginResponse>()
            .WithTags(Tags.Account)
            .WithSummary("Login")
            .WithDescription("Authenticate a user and return a JWT token.");
    }

    private static async Task<IResult> HandleAsync(
        LoginRequest request,
        HalcyonDbContext dbContext,
        ISecretHasher secretHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        CancellationToken cancellationToken = default
    )
    {
        var normalizedEmailAddress = request.EmailAddress.ToLowerInvariant();

        var user = await dbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.NormalizedEmailAddress == normalizedEmailAddress,
                cancellationToken
            );

        if (user is null || user.Password is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        var verified = secretHasher.VerifyHash(request.Password, user.Password);

        if (!verified)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        if (user.IsLockedOut)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "This account has been locked out, please try again later."
            );
        }

        var token = jwtTokenGenerator.GenerateJwtToken(user);
        var result = new LoginResponse(token);

        return Results.Ok(result);
    }
}
