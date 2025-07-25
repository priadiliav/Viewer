using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Services;

public class AgentService(
	IStreamManager streamManager,
	IUnitOfWork unitOfWork) : IAgentService
{
	public async Task<IEnumerable<AgentDto>> GetAllAsync()
	{
		var agentsDomain = await unitOfWork.Agents.GetAllAsync();
		return agentsDomain.Select(x =>
		{
			var isAgentConnected = streamManager.IsAgentConnected(x.Id);
			return x.ToDto(isAgentConnected);
		}).ToList();
	}

	public async Task<AgentDetailsDto?> GetByIdAsync(Guid id)
	{
		var agentDomain = await unitOfWork.Agents.GetByIdAsync(id);
		var isAgentConnected = streamManager.IsAgentConnected(id);
		return agentDomain?.ToDetailsDto(isAgentConnected);
	}

	public async Task<AgentDetailsDto?> CreateAsync(AgentCreateRequest createRequest)
	{
		var agentDomain = createRequest.ToDomain();
        
        var configurationDomain = await unitOfWork.Configurations.GetByIdAsync(agentDomain.ConfigurationId);
        if (configurationDomain is null)
            throw new ArgumentException($"Configuration with ID {agentDomain.ConfigurationId} does not exist.", nameof(agentDomain.ConfigurationId));
        
        configurationDomain.ResetAppliedStatus();
        
		await unitOfWork.Agents.CreateAsync(agentDomain);
		await unitOfWork.SaveChangesAsync();
        
        
		return await GetByIdAsync(agentDomain.Id);
	}

    public async Task<AgentDetailsDto?> UpdateAsync(Guid id, AgentUpdateRequest updateRequest)
    {
        var existingAgent = await unitOfWork.Agents.GetByIdAsync(id);
        if (existingAgent is null)
            throw new ArgumentException($"Agent with ID {id} does not exist.", nameof(id));
        
        var updatedAgentDomain = updateRequest.ToDomain(id);
        existingAgent.UpdateFrom(updatedAgentDomain);
        
        var configurationDomain = await unitOfWork.Configurations.GetByIdAsync(existingAgent.ConfigurationId);
        if (configurationDomain is null)
            throw new ArgumentException($"Configuration with ID {existingAgent.ConfigurationId} does not exist.", nameof(existingAgent.ConfigurationId));
        
        configurationDomain.ResetAppliedStatus();
        
        await unitOfWork.Agents.UpdateAsync(existingAgent);
        await unitOfWork.SaveChangesAsync();
        
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingAgentDomain = await unitOfWork.Agents.GetByIdAsync(id);
        if (existingAgentDomain is null)
            throw new ArgumentException($"Agent with ID {id} does not exist.", nameof(id));
        
        await unitOfWork.Agents.DeleteAsync(existingAgentDomain);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<(string Token, Configuration Configuration)> LoginAsync(string agentId, string agentSecret)
	{
		if (string.IsNullOrWhiteSpace(agentId))
			throw new ArgumentException("Agent ID cannot be null or empty.", nameof(agentId));
		
		if (string.IsNullOrWhiteSpace(agentSecret))
			throw new ArgumentException("Agent secret cannot be null or empty.", nameof(agentSecret));
		
		if (!Guid.TryParse(agentId, out var agentGuid))
			throw new ArgumentException($"Invalid agent ID format: {agentId}", nameof(agentId));
		
		var agentInDbDomain = await unitOfWork.Agents.GetByIdAsync(agentGuid);
		if (agentInDbDomain == null)
			throw new ArgumentException($"Agent with ID {agentId} does not exist.");

		var configurationInDbDomain = await unitOfWork.Configurations.GetByIdAsync(agentInDbDomain.ConfigurationId);
		if (configurationInDbDomain == null)
			throw new ArgumentException($"Configuration with ID {agentInDbDomain.ConfigurationId} does not exist.");
		
		var generatedToken = "generated-token";

		return (
			Token: generatedToken,
			Configuration: configurationInDbDomain
		);
	}

	public async Task<bool> AuthenticateAsync(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
			throw new ArgumentException("Token cannot be null or empty.", nameof(token));

		// Here validate the token against your authentication system.
		// For this example, we will assume the token is valid if it matches a specific string.
		return token == "generated-token"; // Replace with actual token validation logic.
	}
}
