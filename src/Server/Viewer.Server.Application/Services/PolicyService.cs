using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;

namespace Viewer.Server.Application.Services;

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

    public async Task<PolicyDto?> UpdateAsync(long id, PolicyUpdateRequest updateRequest)
    {
        var existingPolicy = await unitOfWork.Policies.GetByIdAsync(id);
        if(existingPolicy is null)
            throw new Exception($"Policy with ID {id} not found.");
        
        var updatedPolicyDomain = updateRequest.ToDomain(id);
        existingPolicy.UpdateFrom(updatedPolicyDomain);
        
        // Reset the applied status of all configurations associated with this policy
        var configurations = await unitOfWork.Configurations.GetByPolicyIdAsync(id);
        foreach (var configuration in configurations)
        {
            configuration.ResetAppliedStatus();
        }
        
        await unitOfWork.Policies.UpdateAsync(existingPolicy);
        await unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(existingPolicy.Id);
    }

    public async Task DeleteAsync(long id)
    {
        var existingPolicy = await unitOfWork.Policies.GetByIdAsync(id);
        if (existingPolicy is null)
            throw new Exception($"Policy with ID {id} not found.");
        
        var isPolicyUsedInConfiguration = await unitOfWork.Configurations.ExistsByPolicyIdAsync(id);
        if(isPolicyUsedInConfiguration)
            throw new Exception($"Policy with ID {id} cannot be deleted because it is used in one or more configurations.");
        
        await unitOfWork.Policies.DeleteAsync(existingPolicy);
        await unitOfWork.SaveChangesAsync();
    }
}
