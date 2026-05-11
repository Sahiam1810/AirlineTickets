using System;
using Domain.Entities.People;

namespace Domain.Entities.Reservations;

public sealed class Reservation
{
    public int Id { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int ReservationStatusId { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Client Client { get; set; } = null!;
    public ReservationStatus ReservationStatus { get; set; } = null!;
    public ICollection<ReservationFlight> ReservationFlights { get; set; } = [];
}