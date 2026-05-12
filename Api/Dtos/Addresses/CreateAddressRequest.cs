namespace Api.Dtos.Addresses;

public sealed class CreateAddressRequest
{
    public int RoadTypeId { get; init; }
    public string StreetName { get; init; } = default!;
    public string? Number { get; init; }
    public string? Complement { get; init; }
    public int CityId { get; init; }
    public string? PostalCode { get; init; }
}
