using System;

namespace Api.Dtos.Reservations;

public sealed class CreateReservationRequest
{
    public string ReservationCode { get; init; } = string.Empty;
    public int ClientId { get; init; }
    public int ReservationStatusId { get; init; }
    public decimal TotalValue { get; init; }
    public DateTime? ExpiresAt { get; init; }
}
