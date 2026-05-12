namespace Api.Dtos.Airports;

public sealed class CreateAirportRequest
{
    public string Name { get; init; } = default!;
    public string IataCode { get; init; } = default!;
    public string? IcaoCode { get; init; }
    public int CityId { get; init; }
}
