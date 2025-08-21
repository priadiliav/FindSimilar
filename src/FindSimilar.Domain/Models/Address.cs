using FindSimilar.Domain.Abstraction;

namespace FindSimilar.Domain.Models;

public class Address : Entity<long>
{
	public string Street { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public string ZipCode { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	
	public void UpdateFrom(Address address)
	{
		Street = address.Street;
		City = address.City;
		State = address.State;
		ZipCode = address.ZipCode;
		Country = address.Country;
	}
}