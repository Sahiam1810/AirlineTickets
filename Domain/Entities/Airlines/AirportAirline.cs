using System;
using Domain.Common;
using Domain.ValueObjects.AirportAirlines;

namespace Domain.Entities.Airlines;

public sealed class AirportAirline : BaseEntity<int>
{
    public int AirportId { get; private set; }
    public int AirlineId { get; private set; }
    public Terminal? Terminal { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public bool IsActive { get; private set; }

    private AirportAirline() { }

    public AirportAirline(
        int airportId,
        int airlineId,
        Terminal? terminal,
        DateOnly startDate,
        DateOnly? endDate,
        bool isActive)
    {
        if (airportId <= 0)
            throw new ArgumentException("Airport id is required", nameof(airportId));

        if (airlineId <= 0)
            throw new ArgumentException("Airline id is required", nameof(airlineId));

        ValidateDates(startDate, endDate);

        AirportId = airportId;
        AirlineId = airlineId;
        Terminal = terminal;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = endDate is null || isActive;
    }

    public void Update(Terminal? terminal, DateOnly startDate, DateOnly? endDate, bool isActive)
    {
        ValidateDates(startDate, endDate);

        Terminal = terminal;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = endDate is null || isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateDates(DateOnly startDate, DateOnly? endDate)
    {
        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be earlier than start date", nameof(endDate));
    }

    // Navigation
    public Airport Airport { get; set; } = null!;
    public Airline Airline { get; set; } = null!;
}
