namespace Api.Dtos.AircraftManufacturers;

public sealed class UpdateAircraftManufacturerRequest
{
    public string Name { get; init; } = default!;
    public string Country { get; init; } = default!;
}
