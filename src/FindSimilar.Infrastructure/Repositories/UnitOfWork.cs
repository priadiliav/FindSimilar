using FindSimilar.Application.Abstractions;
using FindSimilar.Infrastructure.Configs;

namespace FindSimilar.Infrastructure.Repositories;

public class UnitOfWork(
	AppDbContext context,
	IAddressRepository addressRepository) : IUnitOfWork, IDisposable
{
	public IAddressRepository Addresses { get; } = addressRepository;
	
	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await context.SaveChangesAsync(cancellationToken);
	
	public void Dispose() => context.Dispose();
}