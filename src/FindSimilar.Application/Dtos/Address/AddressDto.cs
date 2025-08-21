namespace FindSimilar.Application.Dtos.Address;

public record AddressDto(
	long Id,
	string Street,
	string City,
	string State,
	string ZipCode,
	string Country)
{
	public string ToEmbeddingString() =>
		$"The address is: {Street}, {City}, {State}, {Country} {ZipCode}.";
}