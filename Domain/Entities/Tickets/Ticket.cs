using System;
using Domain.Common;
using Domain.Entities.Reservations;

namespace Domain.Entities.Tickets;

public sealed class Ticket : BaseEntity<int>
{
    public int ReservationPassengerId { get; private set; }
    public string TicketCode { get; private set; } = string.Empty;
    public DateTime IssuedAt { get; private set; }
    public int TicketStatusId { get; private set; }

    // Navigation
    public ReservationPassenger ReservationPassenger { get; set; } = null!;
    public TicketStatus TicketStatus { get; set; } = null!;
    public CheckIn? CheckIn { get; set; }

    private Ticket() { }

    public Ticket(int reservationPassengerId, string ticketCode, DateTime issuedAt, int ticketStatusId)
    {
        ReservationPassengerId = reservationPassengerId;
        TicketCode = ticketCode;
        IssuedAt = issuedAt;
        TicketStatusId = ticketStatusId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(int ticketStatusId)
    {
        TicketStatusId = ticketStatusId;
        UpdatedAt = DateTime.UtcNow;
    }
}