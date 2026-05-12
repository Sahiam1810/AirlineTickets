namespace Api.Dtos.AirportAirlines;

public sealed class CreateAirportAirlineRequest
{
    public int AirportId { get; init; }
    public int AirlineId { get; init; }
    public string? Terminal { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsActive { get; init; }
}
