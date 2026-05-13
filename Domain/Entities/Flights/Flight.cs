using System;
using Domain.Common;
using Domain.Entities.Aircraft;
using Domain.Entities.Airlines;
using Domain.Entities.Routes;
using Domain.ValueObjects.Flights;

namespace Domain.Entities.Flights;

public sealed class Flight : BaseEntity<int>
{
    public FlightCode FlightCode { get; private set; } = null!;
    public int AirlineId { get; private set; }
    public int RouteId { get; private set; }
    public int AircraftId { get; private set; }
    public int FlightStateId { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime EstimatedArrivalDate { get; private set; }
    public Capacity TotalCapacity { get; private set; } = null!;
    public AvailableSeats AvailableSeats { get; private set; } = null!;
    public DateTime? RescheduledAt { get; private set; }

    private Flight() { }

    public Flight(
        string flightCode,
        int airlineId,
        int routeId,
        int aircraftId,
        DateTime departureDate,
        DateTime estimatedArrivalDate,
        int totalCapacity,
        int availableSeats,
        int flightStateId,
        DateTime? rescheduledAt)
    {
        ValidateIds(airlineId, routeId, aircraftId, flightStateId);
        ValidateDates(departureDate, estimatedArrivalDate);

        var capacity = Capacity.Create(totalCapacity);
        var seats = AvailableSeats.Create(availableSeats, capacity);

        FlightCode = FlightCode.Create(flightCode);
        AirlineId = airlineId;
        RouteId = routeId;
        AircraftId = aircraftId;
        DepartureDate = departureDate;
        EstimatedArrivalDate = estimatedArrivalDate;
        TotalCapacity = capacity;
        AvailableSeats = seats;
        FlightStateId = flightStateId;
        RescheduledAt = rescheduledAt;
    }

    public void Update(
        string flightCode,
        int airlineId,
        int routeId,
        int aircraftId,
        DateTime departureDate,
        DateTime estimatedArrivalDate,
        int totalCapacity,
        int availableSeats,
        int flightStateId,
        DateTime? rescheduledAt)
    {
        ValidateIds(airlineId, routeId, aircraftId, flightStateId);
        ValidateDates(departureDate, estimatedArrivalDate);

        var capacity = Capacity.Create(totalCapacity);
        var seats = AvailableSeats.Create(availableSeats, capacity);

        FlightCode = FlightCode.Create(flightCode);
        AirlineId = airlineId;
        RouteId = routeId;
        AircraftId = aircraftId;
        DepartureDate = departureDate;
        EstimatedArrivalDate = estimatedArrivalDate;
        TotalCapacity = capacity;
        AvailableSeats = seats;
        FlightStateId = flightStateId;
        RescheduledAt = rescheduledAt;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeState(int newStateId)
    {
        if (newStateId <= 0)
            throw new ArgumentException("Flight state id is required", nameof(newStateId));

        FlightStateId = newStateId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newDeparture, DateTime newArrival)
    {
        ValidateDates(newDeparture, newArrival);

        DepartureDate = newDeparture;
        EstimatedArrivalDate = newArrival;
        RescheduledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateIds(int airlineId, int routeId, int aircraftId, int flightStateId)
    {
        if (airlineId <= 0)
            throw new ArgumentException("Airline id is required", nameof(airlineId));

        if (routeId <= 0)
            throw new ArgumentException("Route id is required", nameof(routeId));

        if (aircraftId <= 0)
            throw new ArgumentException("Aircraft id is required", nameof(aircraftId));

        if (flightStateId <= 0)
            throw new ArgumentException("Flight state id is required", nameof(flightStateId));
    }

    private static void ValidateDates(DateTime departureDate, DateTime estimatedArrivalDate)
    {
        if (departureDate >= estimatedArrivalDate)
            throw new ArgumentException("Departure date must be earlier than estimated arrival date");
    }

    // Navigation
    public Airline Airline { get; set; } = null!;
    public Route Route { get; set; } = null!;
    public AircraftUnit Aircraft { get; set; } = null!;
    public FlightState FlightState { get; set; } = null!;
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
    public ICollection<FlightSeat> FlightSeats { get; set; } = [];
}
