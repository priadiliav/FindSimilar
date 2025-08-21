namespace FindSimilar.Application.Dtos.Address;

public record CreateAddressRequest(
	string Street,
	string City,
	string State,
	string ZipCode,
	string Country);