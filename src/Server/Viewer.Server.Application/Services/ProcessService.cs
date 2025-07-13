using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Process;
using Viewer.Server.Application.Interfaces.Repositories;

namespace Viewer.Server.Application.Services;

public interface IProcessService
{
	Task<IEnumerable<ProcessDto>> GetAllAsync();
	Task<ProcessDto?> GetByIdAsync(long id);
	Task<ProcessDto?> CreateAsync(ProcessCreateRequest createRequest);
}

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
}