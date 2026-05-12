namespace Api.Dtos.AirportAirlines;

public sealed class AirportAirlineDto
{
    public int Id { get; init; }
    public int AirportId { get; init; }
    public string AirportName { get; init; } = default!;
    public string AirportIataCode { get; init; } = default!;
    public int AirlineId { get; init; }
    public string AirlineName { get; init; } = default!;
    public string AirlineIataCode { get; init; } = default!;
    public string? Terminal { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsActive { get; init; }
}
