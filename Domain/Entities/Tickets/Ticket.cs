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

    private Ticket() { }

    public Ticket(int reservationPassengerId, string ticketCode, DateTime issuedAt, int ticketStatusId)
    {
        Validate(reservationPassengerId, ticketCode, issuedAt, ticketStatusId);

        ReservationPassengerId = reservationPassengerId;
        TicketCode = ticketCode.Trim();
        IssuedAt = issuedAt;
        TicketStatusId = ticketStatusId;
    }

    public void Update(int reservationPassengerId, string ticketCode, DateTime issuedAt, int ticketStatusId)
    {
        Validate(reservationPassengerId, ticketCode, issuedAt, ticketStatusId);

        ReservationPassengerId = reservationPassengerId;
        TicketCode = ticketCode.Trim();
        IssuedAt = issuedAt;
        TicketStatusId = ticketStatusId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(int ticketStatusId)
    {
        if (ticketStatusId <= 0)
        {
            throw new ArgumentException("Ticket status id must be greater than 0.", nameof(ticketStatusId));
        }

        TicketStatusId = ticketStatusId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int reservationPassengerId, string ticketCode, DateTime issuedAt, int ticketStatusId)
    {
        if (reservationPassengerId <= 0)
        {
            throw new ArgumentException("Reservation passenger id must be greater than 0.", nameof(reservationPassengerId));
        }

        if (string.IsNullOrWhiteSpace(ticketCode))
        {
            throw new ArgumentException("Ticket code is required.", nameof(ticketCode));
        }

        if (ticketCode.Trim().Length > 30)
        {
            throw new ArgumentException("Ticket code cannot exceed 30 characters.", nameof(ticketCode));
        }

        if (issuedAt == default)
        {
            throw new ArgumentException("Issued at is required.", nameof(issuedAt));
        }

        if (ticketStatusId <= 0)
        {
            throw new ArgumentException("Ticket status id must be greater than 0.", nameof(ticketStatusId));
        }
    }

    public ReservationPassenger ReservationPassenger { get; set; } = null!;
    public TicketStatus TicketStatus { get; set; } = null!;
    public CheckIn? CheckIn { get; set; }
}
