using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Dtos.Process;

public class ProcessUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public ProcessStatus Status { get; set; }
}
