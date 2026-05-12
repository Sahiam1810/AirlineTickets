namespace Api.Dtos.Fares;

public sealed class UpdateFareRequest
{
    public int RouteId { get; init; }
    public int CabinTypeId { get; init; }
    public int PassengerTypeId { get; init; }
    public int SeasonId { get; init; }
    public decimal BasePrice { get; init; }
    public DateOnly? ValidFrom { get; init; }
    public DateOnly? ValidTo { get; init; }
}
