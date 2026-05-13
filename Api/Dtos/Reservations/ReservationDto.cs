using System;

namespace Api.Dtos.Reservations;

public sealed class ReservationDto
{
    public int Id { get; init; }
    public string ReservationCode { get; init; } = string.Empty;
    public int ClientId { get; init; }
    public DateTime ReservationDate { get; init; }
    public int ReservationStatusId { get; init; }
    public decimal TotalValue { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
