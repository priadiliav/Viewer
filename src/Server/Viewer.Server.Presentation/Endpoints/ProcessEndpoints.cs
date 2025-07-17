using Microsoft.AspNetCore.Mvc;
using Viewer.Server.Application.Dtos.Process;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class ProcessEndpoints
{
	private static async Task<IEnumerable<ProcessDto>> GetProcesses(IProcessService processService)
	{
		var processes = await processService.GetAllAsync();
		return processes;
	}
	
    private static async Task<ProcessDto?> GetProcessById(IProcessService processService, long id)
	{
		var process = await processService.GetByIdAsync(id);
		return process;
	}
	
    private static async Task<ProcessDto?> CreateProcess(IProcessService processService, [FromBody] ProcessCreateRequest createRequest)
	{
		var process = await processService.CreateAsync(createRequest);
		return process;
	}

    private static async Task<ProcessDto?> UpdateProcess(IProcessService processService, long id,
            [FromBody] ProcessUpdateRequest updateRequest)
    {
        var process = await processService.UpdateAsync(id, updateRequest);
        return process;
    }
    
    private static async Task DeleteProcess(IProcessService processService, long id)
    {
        await processService.DeleteAsync(id);
    }
	
	public static void MapProcessEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/processes").WithTags("Processes");
		
		group.MapGet("/", GetProcesses).WithName("GetProcesses")
			.Produces<IEnumerable<ProcessDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all processes");
		
		group.MapGet("/{id:long}", GetProcessById).WithName("GetProcessById")
			.Produces<ProcessDto?>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get process by ID");
		
		group.MapPost("/", CreateProcess).WithName("CreateProcess")
			.Produces<ProcessDto>()
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Create a new process")
			.Accepts<ProcessCreateRequest>("application/json");
        
        group.MapPut("/{id:long}", UpdateProcess).WithName("UpdateProcess")
            .Produces<ProcessDto?>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Update an existing process")
            .Accepts<ProcessUpdateRequest>("application/json");
        
        group.MapDelete("/{id:long}", DeleteProcess).WithName("DeleteProcess")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete a process");
	}
}
