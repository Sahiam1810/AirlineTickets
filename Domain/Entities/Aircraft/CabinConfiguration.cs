using System;
using Domain.Common;
using Domain.ValueObjects.Aircraft;

namespace Domain.Entities.Aircraft;

public sealed class CabinConfiguration : BaseEntity<int>
{
    public int AircraftId { get; private set; }
    public int CabinTypeId { get; private set; }
    public RowNumber RowStart { get; private set; } = null!;
    public RowNumber RowEnd { get; private set; } = null!;
    public SeatsPerRow SeatsPerRow { get; private set; } = null!;
    public SeatLetters SeatLetters { get; private set; } = null!;

    private CabinConfiguration() { }

    public CabinConfiguration(
        int aircraftId,
        int cabinTypeId,
        RowNumber rowStart,
        RowNumber rowEnd,
        SeatsPerRow seatsPerRow,
        SeatLetters seatLetters)
    {
        Validate(aircraftId, cabinTypeId, rowStart, rowEnd, seatsPerRow, seatLetters);

        AircraftId = aircraftId;
        CabinTypeId = cabinTypeId;
        RowStart = rowStart;
        RowEnd = rowEnd;
        SeatsPerRow = seatsPerRow;
        SeatLetters = seatLetters;
    }

    public void Update(
        int aircraftId,
        int cabinTypeId,
        RowNumber rowStart,
        RowNumber rowEnd,
        SeatsPerRow seatsPerRow,
        SeatLetters seatLetters)
    {
        Validate(aircraftId, cabinTypeId, rowStart, rowEnd, seatsPerRow, seatLetters);

        AircraftId = aircraftId;
        CabinTypeId = cabinTypeId;
        RowStart = rowStart;
        RowEnd = rowEnd;
        SeatsPerRow = seatsPerRow;
        SeatLetters = seatLetters;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(
        int aircraftId,
        int cabinTypeId,
        RowNumber rowStart,
        RowNumber rowEnd,
        SeatsPerRow seatsPerRow,
        SeatLetters seatLetters)
    {
        if (aircraftId <= 0)
            throw new ArgumentException("Aircraft id is required", nameof(aircraftId));

        if (cabinTypeId <= 0)
            throw new ArgumentException("Cabin type id is required", nameof(cabinTypeId));

        if (rowEnd.Value <= rowStart.Value)
            throw new ArgumentException("Row end must be greater than row start", nameof(rowEnd));

        if (seatLetters.Value.Length != seatsPerRow.Value)
            throw new ArgumentException("Seat letters length must match seats per row", nameof(seatLetters));
    }

    // Navigation
    public AircraftUnit Aircraft { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
}
