namespace FindSimilar.Application.Abstractions;

public interface IUnitOfWork
{
	IAddressRepository Addresses { get; }
	
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}