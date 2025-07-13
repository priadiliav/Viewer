using Grpc.Net.Client;
using Viewer.Agent.Application.Handlers;
using Viewer.Agent.Application.Interfaces.Handlers;
using Viewer.Agent.Application.Interfaces.Producers;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc.Handlers;
using Viewer.Agent.Infrastructure.Grpc.Producers;
using Viewer.Agent.Infrastructure.Grpc.Services;
using Viewer.Agent.Infrastructure.Repositories;
using Viewer.Agent.Presentation;

var configFile = args.FirstOrDefault(a => a.StartsWith("--config="))?.Split("=")[1]
                 ?? "appsettings.json";

var configuration = new ConfigurationManager();

configuration.AddJsonFile(configFile, optional: false, reloadOnChange: true);

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
		Args = args,
		Configuration = configuration
});

builder.Services.AddGrpc();
builder.Services.AddSingleton(sp =>
{
	var serverUrl = "https://localhost:7041";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.Management.ManagementService.ManagementServiceClient(channel);
});

builder.Services.AddSingleton(sp =>
{
	var serverUrl = "https://localhost:7041";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.StreamService.StreamServiceClient(channel);
});
builder.Services.AddSingleton<IMessageHandlerFactory, GrpcMessageHandlerFactory>();
builder.Services.AddSingleton<IConfigurationHandler, ConfigurationHandler>();

builder.Services.AddSingleton<IConfigurationRepository, JsonConfigurationRepository>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

builder.Services.AddSingleton<IHeartbeatProducer, GrpcHearbeatProducer>();
builder.Services.AddSingleton<IHeartbeatService, HeartbeatService>();

builder.Services.AddSingleton<IManagementServiceClient, GrpcManagementServiceClientClient>();
builder.Services.AddSingleton<IStreamManagerClient, GrpcStreamManagerClient>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<AuthContext>();
builder.Services.Configure<AgentConfig>(builder.Configuration.GetSection("AgentConfig"));
builder.Services.Configure<RepositoryConfig>(builder.Configuration.GetSection("RepositoryConfig"));

var host = builder.Build();
host.Run();