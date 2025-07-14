using Microsoft.Extensions.Logging;
using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Interfaces.Producers;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Services;

public interface IConfigurationProducer : IMessageFromServerProducer<Configuration>;

public interface IConfigurationService
{
	Task<IEnumerable<ConfigurationDto>> GetAllAsync();
	Task<ConfigurationDetailsDto?> GetByIdAsync(long id);
	Task<ConfigurationDetailsDto?> CreateAsync(ConfigurationCreateRequest createRequest);
	Task<ConfigurationDetailsDto> UpdateAsync(long id, ConfigurationUpdateRequest updateRequest);
    Task DeleteAsync(long id);
    Task ApplyConfiguration(long id);
}

public class ConfigurationService(
		ILogger<ConfigurationService> logger,
		IStreamManager streamManager,
		IConfigurationProducer configurationProducer,
		IUnitOfWork unitOfWork) : IConfigurationService
{
	public async Task<IEnumerable<ConfigurationDto>> GetAllAsync()
	{
		var configrationInDb = await unitOfWork.Configurations.GetAllAsync();
		var configurationDtos = configrationInDb.Select(x => x.ToDto());
		return configurationDtos;
	}
	
	public async Task<ConfigurationDetailsDto?> GetByIdAsync(long id)
	{
		var configuration = await unitOfWork.Configurations.GetByIdAsync(id);
		return configuration?.ToDetailsDto();
	}

	public async Task<ConfigurationDetailsDto?> CreateAsync(ConfigurationCreateRequest createRequest)
	{
		var configration = createRequest.ToDomain();
		
		await unitOfWork.Configurations.CreateAsync(configration);
		await unitOfWork.SaveChangesAsync();

		return await GetByIdAsync(configration.Id);
	}
	
	public async Task<ConfigurationDetailsDto> UpdateAsync(long id, ConfigurationUpdateRequest updateRequest)
	{
		var existingConfiguration = await unitOfWork.Configurations.GetByIdAsync(id);
		if (existingConfiguration is null)
			throw new Exception($"Configuration {id} not found");
		
		var updatedConfigurationDomain = updateRequest.ToDomain(id);
		existingConfiguration.UpdateFrom(updatedConfigurationDomain);
        existingConfiguration.ResetAppliedStatus();        
        
		await unitOfWork.Configurations.UpdateAsync(existingConfiguration);
		await unitOfWork.SaveChangesAsync();

		var updatedConfigurationEntity = await unitOfWork.Configurations.GetByIdAsync(id);
		if (updatedConfigurationEntity is null)
			throw new Exception($"Configuration {id} not found after update");
        
		return updatedConfigurationEntity.ToDetailsDto();
	}

    public async Task DeleteAsync(long id)
    {
        var configuration = await unitOfWork.Configurations.GetByIdAsync(id);
        if (configuration is null)
            throw new Exception($"Configuration {id} not found");

        if (configuration.Agents.Any())
            throw new Exception($"Configuration {id} cannot be deleted because it is assigned to agents");
        
        await unitOfWork.Configurations.DeleteAsync(configuration);
        await unitOfWork.SaveChangesAsync();
    }
    

    private async Task ApplyConfigurationToAgentAsync(Guid agentId, Configuration configuration)
	{
        if (!streamManager.IsAgentConnected(agentId))
        {
            logger.LogWarning($"Agent {agentId} is not connected, skipping configuration application");
            return;
        }

        await configurationProducer.ProduceAsync(agentId, configuration);
	}
	
	public async Task ApplyConfiguration(long configurationId)
	{
        var configuration = await unitOfWork.Configurations.GetByIdAsync(configurationId);
        if (configuration is null)
            throw new Exception($"Configuration {configurationId} not found");
        
        var agentIds = configuration.Agents.Select(x => x.Id);
		var tasks = agentIds.Select(agentId => ApplyConfigurationToAgentAsync(agentId, configuration));
        await Task.WhenAll(tasks);
        
        configuration.MarkAsApplied();
        await unitOfWork.Configurations.UpdateAsync(configuration);
        await unitOfWork.SaveChangesAsync();
	}
}
