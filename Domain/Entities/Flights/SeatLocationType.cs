using System;
using Domain.Common;

namespace Domain.Entities.Flights;

public sealed class SeatLocationType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightSeat> FlightSeats { get; set; } = [];
}