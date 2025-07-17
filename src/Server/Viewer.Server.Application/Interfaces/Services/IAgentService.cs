using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Services;

public interface IAgentService
{
    Task<IEnumerable<AgentDto>> GetAllAsync();
    Task<AgentDetailsDto?> GetByIdAsync(Guid id);
    Task<AgentDetailsDto?> CreateAsync(AgentCreateRequest createRequest);
    Task<AgentDetailsDto?> UpdateAsync(Guid id, AgentUpdateRequest updateRequest);
    Task DeleteAsync(Guid id);
    
    Task<(string Token, Configuration Configuration)> LoginAsync(string agentId, string agentSecret);
    Task<bool> AuthenticateAsync(string token);
}
