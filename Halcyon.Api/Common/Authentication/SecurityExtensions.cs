namespace Halcyon.Api.Common.Authentication;

public static class SecurityExtensions
{
    public static IHostApplicationBuilder AddSecurityServices(this IHostApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection(JwtSettings.SectionName);
        builder.Services.Configure<JwtSettings>(jwtConfig);
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        var encryptionConfig = builder.Configuration.GetSection(EncryptionSettings.SectionName);
        builder.Services.Configure<EncryptionSettings>(encryptionConfig);
        builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

        builder.Services.AddSingleton<IHashService, HashService>();

        return builder;
    }
}
