namespace Api.Dtos.Airports;

public sealed class AirportDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string IataCode { get; init; } = default!;
    public string? IcaoCode { get; init; }
    public int CityId { get; init; }
    public string CityName { get; init; } = default!;
}
