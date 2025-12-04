using System.Reflection;
using FluentValidation;
using Halcyon.Api.Common.Authentication;
using Halcyon.Api.Common.Database;
using Halcyon.Api.Common.Email;
using Halcyon.Api.Common.Infrastructure;
using Halcyon.Api.Data;

var assembly = Assembly.GetExecutingAssembly();

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
    builder.Configuration.GetConnectionString("Database")
));

builder.AddFluentEmail(connectionName: "Mail");

var seedConfig = builder.Configuration.GetSection(SeedSettings.SectionName);
builder.Services.Configure<SeedSettings>(seedConfig);
builder.Services.AddHostedService<DbUpMigrationHostedService>();

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddProblemDetails();

builder.ConfigureJsonOptions();
builder.AddAuthentication();
builder.AddSecurityServices();
builder.AddCors();
builder.AddOpenApi(assembly);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApiWithUI(assembly);
app.MapEndpoints(assembly);
app.MapDefaultEndpoints();

app.Run();
