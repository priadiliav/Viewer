using Microsoft.AspNetCore.Mvc;
using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class ConfigurationEndpoints
{
	public static async Task<IEnumerable<ConfigurationDto>> GetConfigurations(IConfigurationService configurationService)
	{
		var configurations = await configurationService.GetAllAsync();
		return configurations;
	}
	
	public static async Task<ConfigurationDetailsDto> CreateConfiguration(IConfigurationService configurationService, 
			[FromBody] ConfigurationCreateRequest createRequest)
	{
		var configuration = await configurationService.CreateAsync(createRequest);
		return configuration;
	}
	
	public static async Task<ConfigurationDetailsDto> UpdateConfiguration(IConfigurationService configurationService, 
			[FromBody] ConfigurationUpdateRequest updateRequest, long id)
	{
		var configuration = await configurationService.UpdateAsync(id, updateRequest);
		return configuration;
	}
	
	public static async Task<ConfigurationDetailsDto?> GetConfigurationById(IConfigurationService configurationService, long id)
	{
		var configuration = await configurationService.GetByIdAsync(id);
		return configuration;
	}
	
	public static void MapConfigurationEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/configurations").WithTags("Configurations");
		
		group.MapGet("/", GetConfigurations).WithName("GetConfigurations")
			.Produces<IEnumerable<ConfigurationDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all configurations");
		
		group.MapGet("/{id:long}", GetConfigurationById).WithName("GetConfigurationById")
			.Produces<ConfigurationDetailsDto?>()
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status200OK)
			.WithSummary("Get configuration by ID");
		
		group.MapPost("/", CreateConfiguration).WithName("CreateConfiguration")
			.Produces<ConfigurationDetailsDto>()
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Create a new configuration")
			.Accepts<ConfigurationCreateRequest>("application/json");
		
		group.MapPut("/{id:long}", UpdateConfiguration).WithName("UpdateConfiguration")
			.Produces<ConfigurationDetailsDto>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Update an existing configuration")
			.Accepts<ConfigurationUpdateRequest>("application/json");
	}
}