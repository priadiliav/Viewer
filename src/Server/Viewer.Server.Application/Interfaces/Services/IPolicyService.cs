using Viewer.Server.Application.Dtos.Policy;

namespace Viewer.Server.Application.Interfaces.Services;

public interface IPolicyService
{
    Task<IEnumerable<PolicyDto>> GetAllAsync();
    Task<PolicyDto?> GetByIdAsync(long id);
    Task<PolicyDto?> CreateAsync(PolicyCreateRequest createRequest);
    Task<PolicyDto?> UpdateAsync(long id, PolicyUpdateRequest updateRequest);
    Task DeleteAsync(long id);
}
