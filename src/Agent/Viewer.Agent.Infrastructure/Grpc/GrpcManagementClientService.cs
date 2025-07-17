using Communication.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using AuthContext = Viewer.Agent.Domain.Configs.AuthContext;

namespace Viewer.Agent.Infrastructure.Grpc;

public interface IManagementClientService
{
	Task LoginAsync(CancellationToken cancellationToken = default);
}
public class GrpcManagementClientService : IManagementClientService
{
	private readonly ILogger<GrpcManagementClientService> _logger;
	private readonly ManagementService.ManagementServiceClient _managementClient;
	private readonly IConfigurationService _configurationService;
	private readonly AuthContext _authContext;
	private readonly AgentConfig _agentConfig;
	
	public GrpcManagementClientService(
		ManagementService.ManagementServiceClient managementClient,
		IConfigurationService configurationService,
		AuthContext authContext,
		IOptions<AgentConfig> agentConfigOptions,
		ILogger<GrpcManagementClientService> logger)
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
