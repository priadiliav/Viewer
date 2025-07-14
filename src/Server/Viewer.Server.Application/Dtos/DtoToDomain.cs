using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Application.Dtos.Heartbeat;
using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Dtos.Process;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Dtos;

public static class DtoToDomain
{
	#region Agent
	public static Domain.Models.Agent ToDomain(this AgentDto agentDto)
	{
		return new Domain.Models.Agent
		{
			Id = agentDto.Id,
			Name = agentDto.Name,
			ConfigurationId = agentDto.ConfigurationId,
		};
	}
	
	public static Domain.Models.Agent ToDomain(this AgentCreateRequest agentCreateRequest)
	{
		return new Domain.Models.Agent
		{
			Name = agentCreateRequest.Name,
			ConfigurationId = agentCreateRequest.ConfigurationId
		};
	}
    
    public static Domain.Models.Agent ToDomain(this AgentUpdateRequest agentUpdateRequest, Guid id)
    {
        return new Domain.Models.Agent
        {
            Id = id,
            Name = agentUpdateRequest.Name,
            ConfigurationId = agentUpdateRequest.ConfigurationId
        };
    }
	#endregion

	#region Heartbeat
	public static Domain.Models.Heartbeat ToDomain(this HeartbeatDto heartbeatDto)
	{
		return new Domain.Models.Heartbeat
		{
			Id = heartbeatDto.Id,
			AgentId = heartbeatDto.AgentId,
			CreatedAt = heartbeatDto.CreatedAt
		};
	}
	#endregion
	
	#region Configuration
	public static Domain.Models.Configuration ToDomain(this ConfigurationDto configurationDto)
	{
		return new Domain.Models.Configuration
		{
			Id = configurationDto.Id,
			Name = configurationDto.Name,
			Agents = configurationDto.AgentIds.Select(id => new Domain.Models.Agent
			{
					Id = id,
					ConfigurationId = configurationDto.Id
			}).ToList() ?? new(),
			Policies = configurationDto.PolicyIds.Select(id => new Domain.Models.PolicyInConfiguration()
			{
					PolicyId = id,
					ConfigurationId = configurationDto.Id
			}).ToList() ?? new(),
			Processes = configurationDto.ProcessIds.Select(id => new Domain.Models.ProcessInConfiguration()
			{
					ProcessId = id, 
					ConfigurationId = configurationDto.Id,
			}).ToList() ?? new()
		};
	}
	
	public static Domain.Models.Configuration ToDetailsDomain(this ConfigurationDetailsDto configurationDetailsDto)
	{
		return new Domain.Models.Configuration
		{
			Id = configurationDetailsDto.Id,
			Name = configurationDetailsDto.Name,
			Agents = configurationDetailsDto.Agents.Select(a => a.ToDomain()).ToList() ?? new(),
			Policies = configurationDetailsDto.Policies.Select(p => new PolicyInConfiguration()
			{
				PolicyId = p.Id,
				ConfigurationId = configurationDetailsDto.Id,
				Policy = p.ToDomain()
			}).ToList() ?? new(),
			Processes = configurationDetailsDto.Processes.Select(p => new ProcessInConfiguration()
			{
				ProcessId = p.Id,
				ConfigurationId = configurationDetailsDto.Id,
				Process = p.ToDomain()
			}).ToList() ?? new()
		};
	}
	
	public static Domain.Models.Configuration ToDomain(this ConfigurationCreateRequest configurationCreateRequest)
	{
		return new Domain.Models.Configuration
		{
			Name = configurationCreateRequest.Name,
			Policies = configurationCreateRequest.PolicyIds.Select(id => new Domain.Models.PolicyInConfiguration
			{
				PolicyId = id,
			}).ToList() ?? new(),
			Processes = configurationCreateRequest.ProcessIds.Select(id => new Domain.Models.ProcessInConfiguration
			{
				ProcessId = id,
			}).ToList() ?? new()
		};
	}
	
	public static Domain.Models.Configuration ToDomain(this ConfigurationUpdateRequest configurationUpdateRequest, long id)
	{
		return new Domain.Models.Configuration
		{
				Id = id,
				Name = configurationUpdateRequest.Name,
				Policies = configurationUpdateRequest.PolicyIds.Select(id => new Domain.Models.PolicyInConfiguration
				{
						PolicyId = id,
				}).ToList() ?? new(),
				Processes = configurationUpdateRequest.ProcessIds.Select(id => new Domain.Models.ProcessInConfiguration
				{
						ProcessId = id,
				}).ToList() ?? new(),
		};
	}

	#endregion
	
	#region Process
	public static Domain.Models.Process ToDomain(this ProcessDto processDto)
	{
		return new Domain.Models.Process
		{
			Id = processDto.Id,
			Name = processDto.Name,
			Status = processDto.Status
		};
	}
	
	public static Domain.Models.Process ToDomain(this ProcessCreateRequest processCreateRequest)
	{
		return new Domain.Models.Process
		{
			Name = processCreateRequest.Name,
			Status = processCreateRequest.Status
		};
	}
    
    public static Domain.Models.Process ToDomain(this ProcessUpdateRequest processUpdateRequest, long id)
    {
        return new Domain.Models.Process
        {
            Id = id,
            Name = processUpdateRequest.Name,
            Status = processUpdateRequest.Status
        };
    }
	#endregion
	
	#region Policy
	public static Domain.Models.Policy ToDomain(this PolicyDto policyDto)
	{
		return new Domain.Models.Policy
		{
			Id = policyDto.Id,
			Name = policyDto.Name,
			Description = policyDto.Description,
			RegistryPath = policyDto.RegistryPath,
			RegistryKeyType = policyDto.RegistryKeyType,
			RegistryKey = policyDto.RegistryKey,
			RegistryValueType = policyDto.RegistryValueType,
			RegistryValue = policyDto.RegistryValue
		};
	}
	
	public static Domain.Models.Policy ToDomain(this PolicyCreateRequest policyCreateRequest)
	{
		return new Domain.Models.Policy
		{
			Name = policyCreateRequest.Name,
			Description = policyCreateRequest.Description,
			RegistryPath = policyCreateRequest.RegistryPath,
			RegistryKeyType = policyCreateRequest.RegistryKeyType,
			RegistryKey = policyCreateRequest.RegistryKey,
			RegistryValueType = policyCreateRequest.RegistryValueType,
			RegistryValue = policyCreateRequest.RegistryValue
		};
	}
    
    public static Domain.Models.Policy ToDomain(this PolicyUpdateRequest policyUpdateRequest, long id)
    {
        return new Domain.Models.Policy
        {
            Id = id,
            Name = policyUpdateRequest.Name,
            Description = policyUpdateRequest.Description,
            RegistryPath = policyUpdateRequest.RegistryPath,
            RegistryKeyType = policyUpdateRequest.RegistryKeyType,
            RegistryKey = policyUpdateRequest.RegistryKey,
            RegistryValueType = policyUpdateRequest.RegistryValueType,
            RegistryValue = policyUpdateRequest.RegistryValue
        };
    }
	#endregion
}

