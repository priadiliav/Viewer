namespace Viewer.Server.Infrastructure.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=yourdb;Username=youruser;Password=yourpassword");
        
		return new AppDbContext(optionsBuilder.Options);
	}
}
