using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Process;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;

namespace Viewer.Server.Application.Services;

public class ProcessService(IUnitOfWork unitOfWork) : IProcessService
{
	public async Task<IEnumerable<ProcessDto>> GetAllAsync()
	{
		var processesDomain = await unitOfWork.Processes.GetAllAsync();
		return processesDomain.Select(x => x.ToDto()).ToList();
	}
	
	public async Task<ProcessDto?> GetByIdAsync(long id)
	{
		var processDomain = await unitOfWork.Processes.GetByIdAsync(id);
		return processDomain?.ToDto();
	}

	public async Task<ProcessDto?> CreateAsync(ProcessCreateRequest createRequest)
	{
		var processDomain = createRequest.ToDomain();
		await unitOfWork.Processes.CreateAsync(processDomain);
		await unitOfWork.SaveChangesAsync();

		return await GetByIdAsync(processDomain.Id);
	}

    public async Task<ProcessDto?> UpdateAsync(long id, ProcessUpdateRequest updateRequest)
    {
        var existingProcess = await unitOfWork.Processes.GetByIdAsync(id);
        if (existingProcess is null)
            throw new Exception($"Process with ID {id} not found.");
        
        var updatedProcessDomain = updateRequest.ToDomain(id);
        existingProcess.UpdateFrom(updatedProcessDomain);
        
        var configurations = await unitOfWork.Configurations.GetByProcessIdAsync(id);
        foreach (var configuration in configurations)
        {
            configuration.ResetAppliedStatus();
        }
        
        await unitOfWork.Processes.UpdateAsync(existingProcess);
        await unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(existingProcess.Id);
    }

    public async Task DeleteAsync(long id)
    {
        var process = await unitOfWork.Processes.GetByIdAsync(id);
        if (process is null)
            throw new Exception($"Process with ID {id} not found.");
        
        var existsInConfigurations = await unitOfWork.Configurations.ExistsByProcessIdAsync(id);
        if (existsInConfigurations)
            throw new Exception($"Process with ID {id} cannot be deleted because it is referenced in configurations.");
        
        await unitOfWork.Processes.DeleteAsync(process);
        await unitOfWork.SaveChangesAsync();
    }
}
