using System;
using Domain.Common;

namespace Domain.Entities.Airlines;

public sealed class AirportAirline : BaseEntity<int>
{
    public int AirportId { get; set; }
    public int AirlineId { get; set; }
    public string? Terminal { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public Airport Airport { get; set; } = null!;
    public Airline Airline { get; set; } = null!;
}