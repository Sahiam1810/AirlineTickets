using System;
using Domain.Common;

namespace Domain.Entities.Reservations;
public sealed class ReservationStatusTransition : BaseEntity<int>
{
    public int FromStatusId { get; private set; }
    public int ToStatusId { get; private set; }

    private ReservationStatusTransition() { }

    public ReservationStatusTransition(int fromStatusId, int toStatusId)
    {
        Validate(fromStatusId, toStatusId);

        FromStatusId = fromStatusId;
        ToStatusId = toStatusId;
    }

    public void Update(int fromStatusId, int toStatusId)
    {
        Validate(fromStatusId, toStatusId);

        FromStatusId = fromStatusId;
        ToStatusId = toStatusId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int fromStatusId, int toStatusId)
    {
        if (fromStatusId <= 0)
            throw new ArgumentException("From status id is required", nameof(fromStatusId));

        if (toStatusId <= 0)
            throw new ArgumentException("To status id is required", nameof(toStatusId));

        if (fromStatusId == toStatusId)
            throw new ArgumentException("From status and to status must be different");
    }

    // Navigation
    public ReservationStatus FromStatus { get; set; } = null!;
    public ReservationStatus ToStatus { get; set; } = null!;
}
