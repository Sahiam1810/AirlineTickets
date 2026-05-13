namespace Api.Dtos.FlightSeats;

public sealed class CreateFlightSeatRequest
{
    public int FlightId { get; init; }
    public string SeatCode { get; init; } = default!;
    public int CabinTypeId { get; init; }
    public int SeatLocationTypeId { get; init; }
    public bool IsOccupied { get; init; }
}
