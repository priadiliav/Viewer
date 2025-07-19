var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var database = builder.AddPostgres("database")
    .WithDataVolume()
    .AddDatabase("ViewerDb");

var api = builder.AddProject<Projects.Viewer_Server_Presentation>("viewer-server-presentation")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpEndpoint(port: 8080, targetPort: 8081, "api");

var ui = builder.AddNpmApp("viewer-ui", "../ui")
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("api"))
    .WithHttpEndpoint(name: "web", port: 59294, isProxied: false)
    .WithExternalHttpEndpoints();

builder.Build().Run();
