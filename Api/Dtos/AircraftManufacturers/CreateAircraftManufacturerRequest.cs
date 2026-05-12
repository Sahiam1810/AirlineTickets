namespace Api.Dtos.AircraftManufacturers;

public sealed class CreateAircraftManufacturerRequest
{
    public string Name { get; init; } = default!;
    public string Country { get; init; } = default!;
}
