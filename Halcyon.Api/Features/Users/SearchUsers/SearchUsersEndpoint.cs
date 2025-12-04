using System.Data;
using Dapper;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Common.Validation;

namespace Halcyon.Api.Features.Users.SearchUsers;

public class SearchUsersEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", HandleAsync)
            .RequireRole(Roles.SystemAdministrator, Roles.UserAdministrator)
            .AddValidationFilter<SearchUsersRequest>()
            .Produces<SearchUsersResponse>()
            .WithTags(Tags.Users)
            .WithSummary("Search Users")
            .WithDescription(
                "Search for user accounts with optional filtering, sorting, and pagination."
            );
    }

    private static async Task<IResult> HandleAsync(
        [AsParameters] SearchUsersRequest request,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = connectionFactory.CreateConnection();

        var page = request.Page ?? 1;
        var size = request.Size ?? 10;
        var offset = (page - 1) * size;

        var searchWhere = string.IsNullOrEmpty(request.Search)
            ? string.Empty
            : "WHERE search_vector @@ websearch_to_tsquery('english', @Search)";

        var orderBy =
            "ORDER BY "
            + request.Sort switch
            {
                UserSort.EMAIL_ADDRESS_DESC => "email_address DESC, id",
                UserSort.EMAIL_ADDRESS_ASC => "email_address ASC, id",
                UserSort.NAME_DESC => "first_name DESC, last_name DESC, id",
                _ => "first_name ASC, last_name ASC, id",
            };

        var countSql = $@"SELECT COUNT(*) FROM users {searchWhere}";
        var count = await connection.ExecuteScalarAsync<int>(countSql, new { request.Search });

        var listSql =
            $@"
            SELECT 
                id, 
                email_address, 
                first_name, 
                last_name, 
                is_locked_out, 
                roles
            FROM
                users
            {searchWhere}
            {orderBy}
            LIMIT @Size OFFSET @Offset";

        var rows = await connection.QueryAsync(
            listSql,
            new
            {
                request.Search,
                Size = size,
                Offset = offset,
            }
        );

        var users = rows.Select(u => new SearchUserResponse(
                u.id,
                u.email_address,
                u.first_name,
                u.last_name,
                u.is_locked_out,
                u.roles
            ))
            .ToList();

        var pageCount = (count + size - 1) / size;
        var hasNextPage = page < pageCount;
        var hasPreviousPage = page > 1 && page <= pageCount;

        var result = new SearchUsersResponse(users, hasNextPage, hasPreviousPage);

        return Results.Ok(result);
    }
}
