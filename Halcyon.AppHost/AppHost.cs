var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("PostgresPassword", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword, port: 5432)
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database", databaseName: "halcyon-dotnet");

var mailpit = builder
    .AddMailPit("mail", httpPort: 8025, smtpPort: 1025)
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder
    .AddProject<Projects.Halcyon_Api>("api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(mailpit)
    .WaitFor(mailpit);

var web = builder
    .AddViteApp("web", "../Halcyon.Web")
    .WithEndpoint("http", e => e.Port = 5173)
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("http"))
    .WithReference(api)
    .WaitFor(api);

api.WithEnvironment("Email__SiteUrl", web.GetEndpoint("http"));

builder.Build().Run();
