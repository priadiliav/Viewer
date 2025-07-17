using Viewer.Server.Application.Dtos;

namespace Viewer.Server.Application.Interfaces.Services;

public interface IConfigurationService
{
    Task<IEnumerable<ConfigurationDto>> GetAllAsync();
    Task<ConfigurationDetailsDto?> GetByIdAsync(long id);
    Task<ConfigurationDetailsDto?> CreateAsync(ConfigurationCreateRequest createRequest);
    Task<ConfigurationDetailsDto> UpdateAsync(long id, ConfigurationUpdateRequest updateRequest);
    Task DeleteAsync(long id);
    Task ApplyConfiguration(long id);
}
