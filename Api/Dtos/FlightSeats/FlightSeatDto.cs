namespace Api.Dtos.FlightSeats;

public sealed class FlightSeatDto
{
    public int Id { get; init; }
    public int FlightId { get; init; }
    public string FlightCode { get; init; } = default!;
    public string SeatCode { get; init; } = default!;
    public int CabinTypeId { get; init; }
    public string CabinTypeName { get; init; } = default!;
    public int SeatLocationTypeId { get; init; }
    public string SeatLocationTypeName { get; init; } = default!;
    public bool IsOccupied { get; init; }
}
