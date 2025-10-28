namespace Halcyon.Api.Features.Users.UnlockUser;

public record UnlockUserRequest(uint? Version);

public record UnlockUserResponse(Guid Id);
