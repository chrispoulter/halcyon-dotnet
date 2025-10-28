namespace Halcyon.Api.Features.Users.LockUser;

public record LockUserRequest(uint? Version);

public record LockUserResponse(Guid Id);
