using FindSimilar.Application.Abstractions;
using FindSimilar.Application.Dtos;
using FindSimilar.Application.Dtos.Address;

namespace FindSimilar.Application.Services;

public interface IAddressService
{
	Task<AddressDto?> CreateAddressAsync(CreateAddressRequest request);
	Task<IEnumerable<AddressDto>> FindNearestAddressesAsync(long addressId, int count = 5);
}

public class AddressService(
	IEmbeddingProvider embeddingProvider,
	IAddressEmbeddingStore addressEmbeddingStore,
	IUnitOfWork unitOfWork) : IAddressService
{
	public async Task<AddressDto?> CreateAddressAsync(CreateAddressRequest request)
	{
		var domainAddress = request.ToDomain();
		
		await unitOfWork.Addresses.CreateAsync(domainAddress);
		await unitOfWork.SaveChangesAsync();
		
		var createdAddress = await unitOfWork.Addresses.GetByIdAsync(domainAddress.Id);
		if (createdAddress is null)
			return null;
		
		var createdAddressDto = createdAddress.ToDto();
		var createdAddressEmbeddingString = createdAddressDto.ToEmbeddingString();
		
		var generatedEmbedding = await embeddingProvider.GenerateEmbeddingAsync(createdAddressEmbeddingString);
		await addressEmbeddingStore.SaveEmbeddingAsync(createdAddress.Id, generatedEmbedding);
		
		return createdAddressDto;
	}

	public async Task<IEnumerable<AddressDto>> FindNearestAddressesAsync(long addressId, int count = 5)
	{
		var addressDomain = await unitOfWork.Addresses.GetByIdAsync(addressId);
		if (addressDomain is null)
			return [];
		
		var addressDto = addressDomain.ToDto();
		var addressEmbeddingString = addressDto.ToEmbeddingString();
		
		var queryEmbedding = await embeddingProvider.GenerateEmbeddingAsync(addressEmbeddingString);
		var nearestAddresses = await addressEmbeddingStore.SearchEmbeddingAsync(queryEmbedding, count);

		var nearestAddressIds = nearestAddresses.Where(x => x.score < 0.5f && x.id != addressId).Select(x => x.id);
		var nearestAddressesDomain = await unitOfWork.Addresses.GetByIdsAsync(nearestAddressIds);
		
		return nearestAddressesDomain.Select(x => x.ToDto());
	}
}