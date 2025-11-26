using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Halcyon.Api.Common.Infrastructure;

public static class OpenApiExtensions
{
    public static IHostApplicationBuilder AddOpenApi(
        this IHostApplicationBuilder builder,
        Assembly assembly
    )
    {
        builder.Services.AddOpenApi(
            "v1",
            options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        var version = assembly.GetSemVerShortSha();

                        document.Info ??= new OpenApiInfo();
                        document.Info.Version = version ?? document.Info.Version;

                        return Task.CompletedTask;
                    }
                );

                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
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

                        return Task.CompletedTask;
                    }
                );

                options.AddOperationTransformer(
                    (operation, context, cancellationToken) =>
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

                        return Task.CompletedTask;
                    }
                );
            }
        );

        return builder;
    }

    public static WebApplication MapOpenApiWithUI(this WebApplication app, Assembly assembly)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(
            "/",
            options =>
            {
                options.Title = assembly.GetName().Name;
            }
        );

        return app;
    }
}
