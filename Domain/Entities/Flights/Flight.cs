using System;
using Domain.Entities.Aircraft;
using Domain.Entities.Airlines;
using Domain.Entities.Routes;

namespace Domain.Entities.Flights;

public sealed class Flight
{
    public int Id { get; set; }
    public string FlightCode { get; set; } = string.Empty;
    public int AirlineId { get; set; }
    public int RouteId { get; set; }
    public int AircraftUnitId { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime EstimatedArrivalDate { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableSeats { get; set; }
    public int FlightStateId { get; set; }
    public DateTime? RescheduledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Airline Airline { get; set; } = null!;
    public Route Route { get; set; } = null!;
    public AircraftUnit AircraftUnit { get; set; } = null!;
    public FlightState FlightState { get; set; } = null!;
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
    public ICollection<FlightSeat> FlightSeats { get; set; } = [];
}