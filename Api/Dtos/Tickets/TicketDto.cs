namespace Api.Dtos.Tickets;

public sealed class TicketDto
{
    public int Id { get; init; }
    public int ReservationPassengerId { get; init; }
    public int ReservationFlightId { get; init; }
    public string ReservationCode { get; init; } = string.Empty;
    public int FlightId { get; init; }
    public string FlightCode { get; init; } = string.Empty;
    public int PassengerId { get; init; }
    public string DocumentNumber { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string TicketCode { get; init; } = string.Empty;
    public DateTime IssuedAt { get; init; }
    public int TicketStatusId { get; init; }
    public string TicketStatusName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
