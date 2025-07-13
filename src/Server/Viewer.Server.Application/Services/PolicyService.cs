using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Interfaces.Repositories;

namespace Viewer.Server.Application.Services;

public interface IPolicyService
{
	Task<IEnumerable<PolicyDto>> GetAllAsync();
	Task<PolicyDto?> GetByIdAsync(long id);
	Task<PolicyDto?> CreateAsync(PolicyCreateRequest createRequest);
}

public class PolicyService(IUnitOfWork unitOfWork) : IPolicyService
{
	public async Task<IEnumerable<PolicyDto>> GetAllAsync()
	{
		var policiesDomain = await unitOfWork.Policies.GetAllAsync();
		return policiesDomain.Select(x => x.ToDto()).ToList();
	}

	public async Task<PolicyDto?> GetByIdAsync(long id)
	{
		var policyDomain = await unitOfWork.Policies.GetByIdAsync(id);
		return policyDomain?.ToDto();
	}

	public async Task<PolicyDto?> CreateAsync(PolicyCreateRequest createRequest)
	{
		var policyDomain = createRequest.ToDomain();
		await unitOfWork.Policies.CreateAsync(policyDomain);
		await unitOfWork.SaveChangesAsync();

		return await GetByIdAsync(policyDomain.Id);
	}
}