using System;

namespace Domain.Entities.Flights;

public sealed class SeatLocationType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightSeat> FlightSeats { get; set; } = [];
}