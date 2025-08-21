using FindSimilar.Application.Abstractions;
using FindSimilar.Domain.Models;
using FindSimilar.Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;

namespace FindSimilar.Infrastructure.Repositories;

public class AddressRepository(AppDbContext appDbContext) : IAddressRepository
{
	public Task CreateAsync(Address address)
	{
		appDbContext.Addresses.Add(address);
		return Task.CompletedTask;
	}

	public async Task<Address?> GetByIdAsync(long id)
		=> await appDbContext.Addresses.FindAsync(id);

	public async Task<IEnumerable<Address>> GetByIdsAsync(IEnumerable<long> ids)
		=> await appDbContext.Addresses.Where(address => ids.Contains(address.Id)).ToListAsync();
}