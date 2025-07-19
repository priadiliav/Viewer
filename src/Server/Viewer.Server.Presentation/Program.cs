using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Handlers;
using Viewer.Server.Application.Interfaces.Handlers;
using Viewer.Server.Application.Interfaces.Producers;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Application.Producers;
using Viewer.Server.Application.Services;
using Viewer.Server.Infrastructure.Configs;
using Viewer.Server.Infrastructure.Grpc;
using Viewer.Server.Infrastructure.Repositories;
using Viewer.Server.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>("ViewerDb");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddGrpc();
//builder.Services.AddDbContext<AppDbContext>(options => 
//		options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IHeartbeatService, HeartbeatService>();

builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IHeartbeatRepository, HeartbeatRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IConfigurationProducer, ConfigurationProducer>();
builder.Services.AddScoped<IHeartbeatHandler, HeartbeatHandler>();

builder.Services.AddScoped<IHandlerResolver, GrpcHandlerResolver>();
builder.Services.AddScoped<IMessageProducer, GrpcMessageProducer>();

builder.Services.AddSingleton<IStreamManager, GrpcStreamManager>();

builder.Services.AddSingleton<GrpcStreamManager>();
builder.Services.AddSingleton<IGrpcStreamManager>(sp => sp.GetRequiredService<GrpcStreamManager>());
builder.Services.AddSingleton<IStreamManager>(sp => sp.GetRequiredService<GrpcStreamManager>());

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Viewer API V1");
		c.RoutePrefix = string.Empty; 
	});
}

app.MapGrpcService<GrpcStreamService>();
app.MapGrpcService<GrpcManagementService>();

app.MapAgentEndpoints();
app.MapConfigurationEndpoints();
app.MapPolicyEndpoints();
app.MapProcessEndpoints();
app.MapHeartbeatEndpoints();
app.MapEnumEndpoints();

app.UseCors("AllowAllOrigins");
app.Run();

