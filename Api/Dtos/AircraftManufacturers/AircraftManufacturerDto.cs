namespace Api.Dtos.AircraftManufacturers;

public sealed class AircraftManufacturerDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Country { get; init; } = default!;
}
