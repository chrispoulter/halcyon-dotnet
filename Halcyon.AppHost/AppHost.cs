var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("PostgresPassword", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword, port: 5432)
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent)
    .PublishAsConnectionString();

var database = postgres.AddDatabase("database", databaseName: "halcyon-dotnet");

var mailpit = builder
    .AddMailPit("mail", httpPort: 8025, smtpPort: 1025)
    .WithLifetime(ContainerLifetime.Persistent)
    .PublishAsConnectionString();

var api = builder
    .AddProject<Projects.Halcyon_Api>("api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(mailpit)
    .WaitFor(mailpit);

var web = builder
    .AddJavaScriptApp("web", "../Halcyon.Web")
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "PORT", port: 5173)
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api)
    .PublishAsDockerFile();

api.WithEnvironment("Email__SiteUrl", web.GetEndpoint("http"));

builder.Build().Run();
