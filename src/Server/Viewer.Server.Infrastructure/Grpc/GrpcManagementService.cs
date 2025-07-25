using Communication.Management;
using Grpc.Core;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Infrastructure.Grpc;

public class GrpcManagementService(IAgentService agentService) : Communication.Management.ManagementService.ManagementServiceBase
{
	public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
	{
		var agentId = request.AgentId;
		var agentSecret = request.AgentSecret;		
		
		var (token, configuration) = await agentService.LoginAsync(agentId, agentSecret);
				
		return new LoginResponse
		{ 
			Token = token, 
			Configuration = configuration.ToGrpc()
		};
	}
}
