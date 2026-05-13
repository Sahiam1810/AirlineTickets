using System;
using Domain.Common;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.FlightSeats;

namespace Domain.Entities.Flights;

public sealed class FlightSeat : BaseEntity<int>
{
    public int FlightId { get; private set; }
    public SeatCode SeatCode { get; private set; } = null!;
    public int CabinTypeId { get; private set; }
    public int SeatLocationTypeId { get; private set; }
    public bool IsOccupied { get; private set; }

    private FlightSeat() { }

    public FlightSeat(
        int flightId,
        string seatCode,
        int cabinTypeId,
        int seatLocationTypeId,
        bool isOccupied)
    {
        ValidateIds(flightId, cabinTypeId, seatLocationTypeId);

        FlightId = flightId;
        SeatCode = SeatCode.Create(seatCode);
        CabinTypeId = cabinTypeId;
        SeatLocationTypeId = seatLocationTypeId;
        IsOccupied = isOccupied;
    }

    public void Update(
        int flightId,
        string seatCode,
        int cabinTypeId,
        int seatLocationTypeId,
        bool isOccupied)
    {
        ValidateIds(flightId, cabinTypeId, seatLocationTypeId);

        FlightId = flightId;
        SeatCode = SeatCode.Create(seatCode);
        CabinTypeId = cabinTypeId;
        SeatLocationTypeId = seatLocationTypeId;
        IsOccupied = isOccupied;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsOccupied()
    {
        IsOccupied = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsAvailable()
    {
        IsOccupied = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateIds(int flightId, int cabinTypeId, int seatLocationTypeId)
    {
        if (flightId <= 0)
            throw new ArgumentException("Flight id is required", nameof(flightId));

        if (cabinTypeId <= 0)
            throw new ArgumentException("Cabin type id is required", nameof(cabinTypeId));

        if (seatLocationTypeId <= 0)
            throw new ArgumentException("Seat location type id is required", nameof(seatLocationTypeId));
    }

    // Navigation
    public Flight Flight { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
    public SeatLocationType SeatLocationType { get; set; } = null!;
}
