using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;
using Halcyon.Api.Data;

namespace Halcyon.Api.Features.Users.UpdateUser;

public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .AddValidationFilter<UpdateUserRequest>()
            .Produces<UpdateUserResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Update User")
            .WithDescription("Update a user account by ID.");
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        UpdateUserRequest request,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            """
            SELECT 
                id AS Id, 
                email_address AS EmailAddress 
            FROM 
                users 
            WHERE 
                id = @Id
            """,
            new { Id = id }
        );

        if (user is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found."
            );
        }

        if (
            !request.EmailAddress.Equals(
                user.EmailAddress,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            var existing = await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS(
                    SELECT 1 FROM users WHERE email_address = @Email
                )
                """,
                new { Email = request.EmailAddress }
            );

            if (existing)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "User name is already taken."
                );
            }
        }

        await connection.ExecuteAsync(
            """
            UPDATE users
            SET email_address = @EmailAddress,
                first_name = @FirstName,
                last_name = @LastName,
                date_of_birth = @DateOfBirth,
                roles = @Roles
            WHERE id = @Id
            """,
            new
            {
                request.EmailAddress,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Roles,
                user.Id,
            }
        );

        return Results.Ok(new UpdateUserResponse(user.Id));
    }
}
