using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace Halcyon.Api.Features.Profile.SetupTwoFactor;

public class SetupTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/profile/setup-two-factor", HandleAsync)
            .RequireAuthorization()
            .Produces<SetupTwoFactorResponse>()
            .WithTags(Tags.Profile);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] SetupTwoFactorRequest request,
        CurrentUser currentUser,
        HalcyonDbContext dbContext,
        IConfiguration configuration,
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

        var rawKey = KeyGeneration.GenerateRandomKey(20);
        var secret = Base32Encoding.ToString(rawKey);

        var issuer = configuration["TwoFactor:Issuer"] ?? "Halcyon";
        var label = $"{issuer}:{user.EmailAddress}";
        var otpAuthUri =
            $"otpauth://totp/{Uri.EscapeDataString(label)}?secret={secret}&issuer={Uri.EscapeDataString(issuer)}&digits=6&period=30";

        user.TwoFactorTempSecret = secret;
        user.IsTwoFactorEnabled = false;

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new SetupTwoFactorResponse(user.Id, secret, otpAuthUri, otpAuthUri);

        return Results.Ok(response);
    }
}
