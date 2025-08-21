using FindSimilar.Application.Dtos.Address;

namespace FindSimilar.Application.Dtos;

public static class ToDtoMapper
{
	#region Address
	public static AddressDto ToDto(this Domain.Models.Address address)
	{
		return new AddressDto(
			address.Id,
			address.Street,
			address.City,
			address.State,
			address.ZipCode,
			address.Country);
	}
	#endregion
}