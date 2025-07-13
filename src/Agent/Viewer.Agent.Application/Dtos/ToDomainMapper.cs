using Viewer.Agent.Domain.Models;
using Process = System.Diagnostics.Process;

namespace Viewer.Agent.Application.Dtos;

public static class ToDomainMapper
{
	public static Domain.Models.Process ToDomain(this Process process)
	{
		return new Domain.Models.Process
		{
				Name = process.ProcessName,
				Status = ProcessStatus.Active 
		};
	}
}