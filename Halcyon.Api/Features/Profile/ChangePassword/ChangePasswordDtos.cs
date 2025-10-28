namespace Halcyon.Api.Features.Profile.ChangePassword;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword, uint? Version);

public record ChangePasswordResponse(Guid Id);
