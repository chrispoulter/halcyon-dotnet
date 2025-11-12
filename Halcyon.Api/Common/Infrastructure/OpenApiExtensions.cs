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
                options.AddDocumentTransformer(
                    async (document, context, cancellationToken) =>
                    {
                        var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                        {
                            [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                In = ParameterLocation.Header,
                                BearerFormat = "Json Web Token",
                            },
                        };

                        document.Components ??= new OpenApiComponents();
                        document.Components.SecuritySchemes = securitySchemes;
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
                            operation.Security ??= [];
                            operation.Security.Add(
                                new OpenApiSecurityRequirement
                                {
                                    [
                                        new OpenApiSecuritySchemeReference(
                                            JwtBearerDefaults.AuthenticationScheme,
                                            context.Document
                                        )
                                    ] = [],
                                }
                            );
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
