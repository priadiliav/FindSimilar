using FindSimilar.Application.Dtos.Address;
using FindSimilar.Application.Services;

namespace FindSimilar.WebApi.Endpoints;

public static class AddressEndpoints
{
	public static void MapAddressEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/addresses").WithTags("Addresses");
		
		group.MapPost("/", async (CreateAddressRequest request, IAddressService addressService) =>
		{
			var createdAddress = await addressService.CreateAddressAsync(request);
			return createdAddress is not null 
					? Results.Created($"/addresses/{createdAddress.Id}", createdAddress) 
					: Results.BadRequest("Failed to create address.");
		});
		
		group.MapGet("/{addressId:long}/nearest", async (long addressId, int count, IAddressService addressService) =>
		{
			var nearestAddresses = await addressService.FindNearestAddressesAsync(addressId, count);
			return Results.Ok(nearestAddresses);
		});
	}
}