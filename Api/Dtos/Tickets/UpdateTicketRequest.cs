namespace Api.Dtos.Tickets;

public sealed class UpdateTicketRequest
{
    public int ReservationPassengerId { get; init; }
    public string TicketCode { get; init; } = string.Empty;
    public DateTime IssuedAt { get; init; }
    public int TicketStatusId { get; init; }
}
