using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Application.Dtos.Heartbeat;

namespace Viewer.Server.Application.Dtos;

public static class DomainToDto
{
	#region Agent
	public static AgentDto ToDto(this Domain.Models.Agent agent, bool isConnected = false)
	{
		return new AgentDto
		{
			Id = agent.Id,
			Name = agent.Name,
			ConfigurationId = agent.ConfigurationId,
			IsConnected = isConnected,
		};
	}
	
	public static AgentDetailsDto ToDetailsDto(this Domain.Models.Agent agent, bool isConnected = false)
	{
		return new AgentDetailsDto
		{
			Id = agent.Id,
			Name = agent.Name,
			CreatedAt = agent.CreatedAt,
			IsConnected = isConnected,
			UpdatedAt = agent.UpdatedAt,
			Configuration = agent.Configuration?.ToDto() ?? new ConfigurationDto()
		};
	}
	#endregion

	#region Heartbeat
	public static HeartbeatDto ToDto(this Domain.Models.Heartbeat heartbeat)
	{
		return new Dtos.Heartbeat.HeartbeatDto
		{
			Id = heartbeat.Id,
			AgentId = heartbeat.AgentId,
			CreatedAt = heartbeat.CreatedAt,
		};
	}
	#endregion
	
	#region Configuration
	public static ConfigurationDto ToDto(this Domain.Models.Configuration configuration)
	{
		return new ConfigurationDto
		{
			Id = configuration.Id,
			Name = configuration.Name,
            IsApplied = configuration.IsApplied,
			AgentIds = configuration.Agents.Select(a => a.Id).ToList(),
			PolicyIds = configuration.Policies.Select(p => p.PolicyId).ToList(),
			ProcessIds = configuration.Processes.Select(p => p.ProcessId).ToList()
		};
	}
	
	public static ConfigurationDetailsDto ToDetailsDto(this Domain.Models.Configuration configuration)
	{
		return new ConfigurationDetailsDto
		{
			Id = configuration.Id,
			Name = configuration.Name,
            IsApplied = configuration.IsApplied,
			Agents = configuration.Agents.Select(a => a.ToDto()).ToList(),
			Policies = configuration.Policies.Select(p => p.Policy.ToDto()).ToList(),
			Processes = configuration.Processes.Select(p => p.Process.ToDto()).ToList()
		};
	}
	#endregion
	
	#region Process
	public static Dtos.Process.ProcessDto ToDto(this Domain.Models.Process process)
	{
		return new Dtos.Process.ProcessDto
		{
			Id = process.Id,
			Name = process.Name,
			Status = process.Status
		};
	}
	#endregion
	
	#region Policy
	public static Dtos.Policy.PolicyDto ToDto(this Domain.Models.Policy policy)
	{
		return new Dtos.Policy.PolicyDto
		{
			Id = policy.Id,
			Name = policy.Name,
			Description = policy.Description,
			RegistryPath = policy.RegistryPath,
			RegistryKey = policy.RegistryKey,
			RegistryKeyType = policy.RegistryKeyType,
			RegistryValueType = policy.RegistryValueType,
			RegistryValue = policy.RegistryValue
		};
	}
	#endregion
}
