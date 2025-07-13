using Grpc.Net.Client;
using Microsoft.Extensions.Options;
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

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ConnectionConfig>(builder.Configuration.GetSection("ConnectionConfig"));
builder.Services.Configure<AgentConfig>(builder.Configuration.GetSection("AgentConfig"));
builder.Services.Configure<RepositoryConfig>(builder.Configuration.GetSection("RepositoryConfig"));

builder.Services.AddGrpc();
builder.Services.AddSingleton(sp =>
{
	var connectionConfig = sp.GetRequiredService<IOptions<ConnectionConfig>>().Value;
	var serverUrl = $"{(connectionConfig.UseSsl ? "https" : "http")}://{connectionConfig.Host}:{connectionConfig.Port}";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.Management.ManagementService.ManagementServiceClient(channel);
});

builder.Services.AddSingleton(sp =>
{
	var connectionConfig = sp.GetRequiredService<IOptions<ConnectionConfig>>().Value;
	var serverUrl = $"{(connectionConfig.UseSsl ? "https" : "http")}://{connectionConfig.Host}:{connectionConfig.Port}";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.StreamService.StreamServiceClient(channel);
});

builder.Services.AddSingleton<IMessageHandlerFactory, GrpcMessageHandlerFactory>();
builder.Services.AddSingleton<IConfigurationHandler, ConfigurationHandler>();

builder.Services.AddSingleton<IPolicyRepository, JsonPolicyRepository>();
builder.Services.AddSingleton<IProcessRepository, JsonProcessRepository>();
builder.Services.AddSingleton<IPolicyService, PolicyService>();
builder.Services.AddSingleton<IProcessService, ProcessService>();

builder.Services.AddSingleton<IConfigurationRepository, JsonConfigurationRepository>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

builder.Services.AddSingleton<IHeartbeatProducer, GrpcHearbeatProducer>();
builder.Services.AddSingleton<IHeartbeatService, HeartbeatService>();

builder.Services.AddSingleton<IManagementServiceClient, GrpcManagementServiceClientClient>();
builder.Services.AddSingleton<IStreamManagerClient, GrpcStreamManagerClient>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<AuthContext>();

var host = builder.Build();
host.Run();