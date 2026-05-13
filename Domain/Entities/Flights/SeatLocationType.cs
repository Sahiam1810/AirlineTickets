using System;
using Domain.Common;
using Domain.ValueObjects.SeatLocationTypes;

namespace Domain.Entities.Flights;

public sealed class SeatLocationType : BaseEntity<int>
{
    public SeatLocationTypeName Name { get; private set; } = null!;

    private SeatLocationType() { }

    public SeatLocationType(string name)
    {
        Name = SeatLocationTypeName.Create(name);
    }

    public void Update(string name)
    {
        Name = SeatLocationTypeName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<FlightSeat> FlightSeats { get; set; } = [];
}
