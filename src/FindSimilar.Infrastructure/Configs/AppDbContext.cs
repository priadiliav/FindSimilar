using FindSimilar.Domain.Abstraction;
using FindSimilar.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FindSimilar.Infrastructure.Configs;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
		public DbSet<Domain.Models.Address> Addresses { get; set; }
		
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
      var entries = ChangeTracker.Entries()
          .Where(e => e is { Entity: ITrackable, State: EntityState.Added or EntityState.Modified });

      var now = DateTimeOffset.UtcNow;

      foreach (var entry in entries)
      {
        var entity = (ITrackable)entry.Entity;
        switch (entry.State)
        {
          case EntityState.Added:
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
            break;
          case EntityState.Modified:
            entity.UpdatedAt = now;
            break;
          case EntityState.Detached:
            break;
          case EntityState.Unchanged:
            break;
          case EntityState.Deleted:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}