namespace Api.Dtos.Flights;

public sealed class RescheduleFlightRequest
{
    public DateTime DepartureDate { get; init; }
    public DateTime EstimatedArrivalDate { get; init; }
}
