﻿namespace Halcyon.Api.Common.Authentication;

public record CurrentUser(Guid Id)
{
    public static ValueTask<CurrentUser?> BindAsync(HttpContext httpContext)
    {
        if (!Guid.TryParse(httpContext.User.Identity?.Name, out var id))
        {
            return ValueTask.FromResult<CurrentUser?>(null);
        }

        return ValueTask.FromResult<CurrentUser?>(new CurrentUser(id));
    }
}
