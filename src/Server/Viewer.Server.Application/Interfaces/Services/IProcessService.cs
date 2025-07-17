using Viewer.Server.Application.Dtos.Process;

namespace Viewer.Server.Application.Interfaces.Services;

public interface IProcessService
{
    Task<IEnumerable<ProcessDto>> GetAllAsync();
    Task<ProcessDto?> GetByIdAsync(long id);
    Task<ProcessDto?> CreateAsync(ProcessCreateRequest createRequest);
    Task<ProcessDto?> UpdateAsync(long id, ProcessUpdateRequest updateRequest);
    Task DeleteAsync(long id);
}
