namespace Api.Dtos.AirportAirlines;

public sealed class UpdateAirportAirlineRequest
{
    public string? Terminal { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsActive { get; init; }
}
