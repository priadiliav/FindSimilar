using FindSimilar.Application.Dtos.Address;

namespace FindSimilar.Application.Dtos;

public static class ToDomainMapper
{
	#region Address
	public static Domain.Models.Address ToDomain(this CreateAddressRequest request)
	{
		return new Domain.Models.Address
		{
			Street = request.Street,
			City = request.City,
			State = request.State,
			ZipCode = request.ZipCode,
			Country = request.Country,
		};
	}
	
	public static Domain.Models.Address ToDomain(this UpdateAddressRequest request)
	{
		return new Domain.Models.Address
		{
			Street = request.Street,
			City = request.City,
			State = request.State,
			ZipCode = request.ZipCode,
			Country = request.Country
		};
	}
	#endregion
}