namespace Api.Dtos.Fares;

public sealed class FareDto
{
    public int Id { get; init; }
    public int RouteId { get; init; }
    public int OriginAirportId { get; init; }
    public string OriginAirportName { get; init; } = default!;
    public string OriginAirportIataCode { get; init; } = default!;
    public int DestinationAirportId { get; init; }
    public string DestinationAirportName { get; init; } = default!;
    public string DestinationAirportIataCode { get; init; } = default!;
    public int CabinTypeId { get; init; }
    public string CabinTypeName { get; init; } = default!;
    public int PassengerTypeId { get; init; }
    public string PassengerTypeName { get; init; } = default!;
    public int SeasonId { get; init; }
    public string SeasonName { get; init; } = default!;
    public decimal BasePrice { get; init; }
    public DateOnly? ValidFrom { get; init; }
    public DateOnly? ValidTo { get; init; }
}
