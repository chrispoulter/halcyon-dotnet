namespace Halcyon.Api.Features;

public class UpdateRequest
{
    public uint? Version { get; set; }
}

public class UpdateResponse
{
    public Guid Id { get; set; }
}
