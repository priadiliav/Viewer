namespace Viewer.Server.Infrastructure.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseNpgsql("Host=localhost;Port=56758;Username=postgres;Password=svgzzs{!fxQD4Y6sQUkCMM;Database=ViewerDb");
        
		return new AppDbContext(optionsBuilder.Options);
	}
}
