using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace Halcyon.Api.Common.Infrastructure;

public static class OpenApiExtensions
{
    public static IHostApplicationBuilder AddOpenApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(
            "v1",
            options =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                };

                options.AddDocumentTransformer(
                    async (document, context, cancellationToken) =>
                    {
                        document.Servers?.Clear();
                        document.Components ??= new();
                        document.Components.SecuritySchemes ??=
                            new Dictionary<string, IOpenApiSecurityScheme>();

                        document.Components.SecuritySchemes.Add(
                            JwtBearerDefaults.AuthenticationScheme,
                            scheme
                        );
                    }
                );

                options.AddOperationTransformer(
                    async (operation, context, cancellationToken) =>
                    {
                        if (
                            context
                                .Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>()
                                .Any()
                        )
                        {
                            var schemeRef = new OpenApiSecuritySchemeReference(
                                JwtBearerDefaults.AuthenticationScheme,
                                context.Document
                            );

                            operation.Security = [new() { [schemeRef] = [] }];
                        }
                    }
                );
            }
        );

        return builder;
    }

    public static WebApplication MapOpenApiWithSwagger(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/openapi/v1.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
