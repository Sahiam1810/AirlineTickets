namespace Api.Dtos.Aircraft;

public sealed class UpdateAircraftRequest
{
    public int AircraftModelId { get; init; }
    public int AirlineId { get; init; }
    public string Registration { get; init; } = default!;
    public DateOnly? ManufactureDate { get; init; }
    public bool IsActive { get; init; }
}
