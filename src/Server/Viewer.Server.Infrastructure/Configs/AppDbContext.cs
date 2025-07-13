using Microsoft.EntityFrameworkCore;
using Viewer.Server.Domain;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Infrastructure.Configs;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}
	public DbSet<Domain.Models.Agent> Agents { get; set; } = null!;
	public DbSet<Domain.Models.Configuration> Configurations { get; set; } = null!;
	public DbSet<Domain.Models.Policy> Policies { get; set; } = null!;
	public DbSet<Domain.Models.Process> Proceses { get; set; } = null!;
	public DbSet<Domain.Models.Heartbeat> Heartbeats { get; set; } = null!;
	
	#region Overrides
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		
		modelBuilder.Entity<PolicyInConfiguration>()
				.HasKey(pc => new { pc.ConfigurationId, pc.PolicyId });
		
		modelBuilder.Entity<PolicyInConfiguration>()
				.HasOne(pc => pc.Configuration)
				.WithMany(c => c.Policies)
				.HasForeignKey(pc => pc.ConfigurationId);

		modelBuilder.Entity<PolicyInConfiguration>()
				.HasOne(pc => pc.Policy)
				.WithMany(p => p.Configurations)
				.HasForeignKey(pc => pc.PolicyId);

		modelBuilder.Entity<ProcessInConfiguration>()
				.HasKey(pc => new { pc.ConfigurationId, ProcesId = pc.ProcessId });

		modelBuilder.Entity<ProcessInConfiguration>()
				.HasOne(pc => pc.Configuration)
				.WithMany(c => c.Processes)
				.HasForeignKey(pc => pc.ConfigurationId);

		modelBuilder.Entity<ProcessInConfiguration>()
				.HasOne(pc => pc.Process)
				.WithMany(p => p.Configurations)
				.HasForeignKey(pc => pc.ProcessId);
	}

	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
	{
		var entries = ChangeTracker.Entries()
				.Where(e => e.Entity is ITrackable 
				            && (e.State == EntityState.Added || e.State == EntityState.Modified));

		var now = DateTimeOffset.UtcNow;

		foreach (var entry in entries)
		{
			var entity = (ITrackable)entry.Entity;

			if (entry.State == EntityState.Added)
			{
				entity.CreatedAt = now;
				entity.UpdatedAt = now;
			}
			else if (entry.State == EntityState.Modified)
			{
				entity.UpdatedAt = now;
			}
		}

		return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}
	#endregion
}