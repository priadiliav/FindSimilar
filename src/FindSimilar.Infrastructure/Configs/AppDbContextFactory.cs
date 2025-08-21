using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FindSimilar.Infrastructure.Configs;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FindSimilar;Username=postgres;Password=postgres");
        
		return new AppDbContext(optionsBuilder.Options);
	}
}