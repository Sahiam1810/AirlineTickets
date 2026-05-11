using System;
using Domain.Common;
using Domain.Entities.Aircraft;

namespace Domain.Entities.Flights;

public sealed class FlightSeat : BaseEntity<int>
{
    public int FlightId { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public int CabinTypeId { get; set; }
    public int SeatLocationTypeId { get; set; }
    public bool IsOccupied { get; set; }

    // Navigation
    public Flight Flight { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
    public SeatLocationType SeatLocationType { get; set; } = null!;
}