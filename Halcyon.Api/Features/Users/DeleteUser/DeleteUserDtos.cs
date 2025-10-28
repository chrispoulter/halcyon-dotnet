namespace Halcyon.Api.Features.Users.DeleteUser;

public record DeleteUserRequest(uint? Version);

public record DeleteUserResponse(Guid Id);
