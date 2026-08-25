using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Telemetry;
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
        IHashService hashService,
        IJwtService jwtService,
        AppMetrics appMetrics,
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
            appMetrics.RecordLoginAttempt("invalid_credentials");

            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        var verified = hashService.VerifyHash(request.Password, user.Password);

        if (!verified)
        {
            appMetrics.RecordLoginAttempt("invalid_credentials");

            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "The credentials provided were invalid."
            );
        }

        if (user.IsLockedOut)
        {
            appMetrics.RecordLoginAttempt("locked_out");

            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "This account has been locked out, please try again later."
            );
        }

        var token = jwtService.GenerateJwtToken(user);
        var result = new LoginResponse(token);

        appMetrics.RecordLoginAttempt("success");

        return Results.Ok(result);
    }
}
