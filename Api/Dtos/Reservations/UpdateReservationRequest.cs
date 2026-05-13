using System;

namespace Api.Dtos.Reservations;

public sealed class UpdateReservationRequest
{
    public int ReservationStatusId { get; init; }
    public decimal TotalValue { get; init; }
    public DateTime? ExpiresAt { get; init; }
}
