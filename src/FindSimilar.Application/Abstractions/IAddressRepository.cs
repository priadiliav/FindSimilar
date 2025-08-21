using FindSimilar.Domain.Models;

namespace FindSimilar.Application.Abstractions;

public interface IAddressRepository
{
	Task CreateAsync(Address address);
	Task<Address?> GetByIdAsync(long id);
	Task<IEnumerable<Address>> GetByIdsAsync(IEnumerable<long> ids);
}