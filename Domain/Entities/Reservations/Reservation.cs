using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Entities.Payments;
using Domain.Entities.People;
using Domain.ValueObjects.Reservations;

namespace Domain.Entities.Reservations;

public sealed class Reservation : BaseEntity<int>
{
    public ReservationCode ReservationCode { get; private set; } = null!;
    public int ClientId { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public int ReservationStatusId { get; private set; }
    public TotalValue TotalValue { get; private set; } = null!;
    public DateTime? ExpiresAt { get; private set; }

    public Client Client { get; set; } = null!;
    public ReservationStatus ReservationStatus { get; set; } = null!;
    public ICollection<ReservationFlight> ReservationFlights { get; set; } = new List<ReservationFlight>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    private Reservation() { }

    public Reservation(string reservationCode, int clientId, int reservationStatusId, decimal totalValue, DateTime? expiresAt)
    {
        if (clientId <= 0) throw new ArgumentException("Client ID must be greater than 0.");
        if (reservationStatusId <= 0) throw new ArgumentException("Reservation Status ID must be greater than 0.");

        ReservationCode = ReservationCode.Create(reservationCode);
        ClientId = clientId;
        ReservationDate = DateTime.UtcNow;
        ReservationStatusId = reservationStatusId;
        TotalValue = TotalValue.Create(totalValue);
        ExpiresAt = expiresAt;
    }

    public void Update(int reservationStatusId, decimal totalValue, DateTime? expiresAt)
    {
        if (reservationStatusId <= 0) throw new ArgumentException("Reservation Status ID must be greater than 0.");
        
        ReservationStatusId = reservationStatusId;
        TotalValue = TotalValue.Create(totalValue);
        ExpiresAt = expiresAt;
        UpdatedAt = DateTime.UtcNow;
    }
}