using Communication.Common;
using Communication.Management;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc.Mappers;
using Viewer.Agent.Infrastructure.Grpc.Utils;
using AuthContext = Viewer.Agent.Domain.Configs.AuthContext;

namespace Viewer.Agent.Infrastructure.Grpc.Services;

public interface IManagementServiceClient
{
	Task LoginAsync(CancellationToken cancellationToken = default);
}
public class GrpcManagementServiceClientClient : IManagementServiceClient
{
	private readonly ILogger<GrpcManagementServiceClientClient> _logger;
	private readonly ManagementService.ManagementServiceClient _managementClient;
	private readonly IConfigurationService _configurationService;
	private readonly AuthContext _authContext;
	private readonly AgentConfig _agentConfig;
	
	public GrpcManagementServiceClientClient(
		ManagementService.ManagementServiceClient managementClient,
		IConfigurationService configurationService,
		AuthContext authContext,
		IOptions<AgentConfig> agentConfigOptions,
		ILogger<GrpcManagementServiceClientClient> logger)
	{
		this._managementClient = managementClient;
		this._configurationService = configurationService;
		this._authContext = authContext;
		this._agentConfig = agentConfigOptions.Value;
		this._logger = logger;
	}
	
	/// <summary>
	/// Login to the management service and applying the configuration.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public async Task LoginAsync(CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(_agentConfig.Id) || string.IsNullOrWhiteSpace(_agentConfig.Secret))
		{
			_logger.LogError("Agent ID or secret is empty.");
			throw new InvalidOperationException("Agent ID and secret must be provided.");
		}
		
		var request = new LoginRequest
		{
			AgentId = _agentConfig.Id,
			AgentSecret = _agentConfig.Secret
		};
		
		var call = _managementClient.LoginAsync(request, cancellationToken: cancellationToken);
		
		var response = await call.ResponseAsync;
		
		_authContext.Token = response.Token.Replace("Bearer ", string.Empty);
		
		// replace by the func of mapping 
		var configurationDomain = response.Configuration.ToDomain();
		
		// apply configurations
		await _configurationService.ApplyConfigurationAsync(configurationDomain);
		
		_logger.LogInformation("Successfully authenticated agent");
	}
}