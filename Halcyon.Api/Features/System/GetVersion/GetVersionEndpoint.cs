using System.Reflection;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;

namespace Halcyon.Api.Features.System.GetVersion;

public class GetVersionEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/system/version", HandleAsync)
            .WithTags(Tags.System)
            .Produces<GetVersionResponse>();
    }

    private static IResult HandleAsync(
        Guid id,
        HalcyonDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var version = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        var result = new GetVersionResponse
        {
            Version = version,
            UtcNow = DateTime.UtcNow,
            UtcNowDateOnly = DateOnly.FromDateTime(DateTime.UtcNow),
            Now = DateTime.Now,
            NowDateOnly = DateOnly.FromDateTime(DateTime.Now),
        };

        return Results.Ok(result);
    }
}
