namespace Api.Dtos.Aircraft;

public sealed class AircraftDto
{
    public int Id { get; init; }
    public int AircraftModelId { get; init; }
    public string AircraftModelName { get; init; } = default!;
    public int AirlineId { get; init; }
    public string AirlineName { get; init; } = default!;
    public string Registration { get; init; } = default!;
    public DateOnly? ManufactureDate { get; init; }
    public bool IsActive { get; init; }
}
