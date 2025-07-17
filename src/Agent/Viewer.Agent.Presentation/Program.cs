using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Handlers;
using Viewer.Agent.Application.Interfaces.Handlers;
using Viewer.Agent.Application.Interfaces.Producers;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Application.Producers;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc;
using Viewer.Agent.Infrastructure.Repositories;
using Viewer.Agent.Presentation;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ConnectionConfig>(builder.Configuration.GetSection("ConnectionConfig"));
builder.Services.Configure<AgentConfig>(builder.Configuration.GetSection("AgentConfig"));
builder.Services.Configure<RepositoryConfig>(builder.Configuration.GetSection("RepositoryConfig"));

builder.Services.AddGrpc();
builder.Services.AddSingleton(sp =>
{
    // Always use the https, because grpc streaming
	var connectionConfig = sp.GetRequiredService<IOptions<ConnectionConfig>>().Value;
	var serverUrl = $"https://{connectionConfig.Host}:{connectionConfig.Port}";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.Management.ManagementService.ManagementServiceClient(channel);
});

builder.Services.AddSingleton(sp =>
{
    // Always use the https, because grpc streaming
	var connectionConfig = sp.GetRequiredService<IOptions<ConnectionConfig>>().Value;
	var serverUrl = $"https://{connectionConfig.Host}:{connectionConfig.Port}";
	var channel = GrpcChannel.ForAddress(serverUrl);
	return new Communication.StreamService.StreamServiceClient(channel);
});

builder.Services.AddSingleton<IHandlerResolver, GrpcHandlerResolver>();
builder.Services.AddSingleton<IMessageProducer, GrpcMessageProducer>();

builder.Services.AddSingleton<IConfigurationHandler, ConfigurationHandler>();
builder.Services.AddSingleton<IHeartbeatProducer, HeartbeatProducer>();

builder.Services.AddSingleton<IPolicyRepository, JsonPolicyRepository>();
builder.Services.AddSingleton<IProcessRepository, JsonProcessRepository>();
builder.Services.AddSingleton<IPolicyService, PolicyService>();
builder.Services.AddSingleton<IProcessService, ProcessService>();

builder.Services.AddSingleton<IConfigurationRepository, JsonConfigurationRepository>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

builder.Services.AddSingleton<IHeartbeatService, HeartbeatService>();

builder.Services.AddSingleton<IManagementClientService, GrpcManagementClientService>();
builder.Services.AddSingleton<IStreamClientManager, GrpcStreamClientManager>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<AuthContext>();

var host = builder.Build();
host.Run();
