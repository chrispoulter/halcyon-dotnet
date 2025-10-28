namespace Halcyon.Api.Features.Account.ResetPassword;

public record ResetPasswordRequest(Guid Token, string EmailAddress, string NewPassword);

public record ResetPasswordResponse(Guid Id);
