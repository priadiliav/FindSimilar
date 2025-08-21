namespace FindSimilar.Application.Dtos.Address;

public record UpdateAddressRequest (
	string Street,
	string City,
	string State,
	string ZipCode,
	string Country);