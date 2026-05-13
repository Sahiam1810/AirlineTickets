using System;
using Domain.Common;
using Domain.Entities.Airlines;
using Domain.Entities.Flights;
using Domain.Entities.People;
using Domain.Entities.Tickets;
using Domain.ValueObjects.Staff;

namespace Domain.Entities.Staff;

public sealed class StaffMember : BaseEntity<int>
{
    public int PersonId { get; private set; }
    public int StaffRoleId { get; private set; }
    public int? AirlineId { get; private set; }
    public int? AirportId { get; private set; }
    public HireDate HireDate { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private StaffMember() { }

    public StaffMember(int personId, int staffRoleId, int? airlineId, int? airportId, HireDate hireDate, bool isActive)
    {
        if (personId <= 0)
            throw new ArgumentException("Person id is required", nameof(personId));

        Validate(staffRoleId, airlineId, airportId);

        PersonId = personId;
        StaffRoleId = staffRoleId;
        AirlineId = airlineId;
        AirportId = airportId;
        HireDate = hireDate;
        IsActive = isActive;
    }

    public void Update(int staffRoleId, int? airlineId, int? airportId, HireDate hireDate, bool isActive)
    {
        Validate(staffRoleId, airlineId, airportId);

        StaffRoleId = staffRoleId;
        AirlineId = airlineId;
        AirportId = airportId;
        HireDate = hireDate;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int staffRoleId, int? airlineId, int? airportId)
    {
        if (staffRoleId <= 0)
            throw new ArgumentException("Staff role id is required", nameof(staffRoleId));

        if (!airlineId.HasValue && !airportId.HasValue)
            throw new ArgumentException("Airline id or airport id is required");

        if (airlineId.HasValue && airportId.HasValue)
            throw new ArgumentException("Staff cannot belong to an airline and an airport at the same time");
    }

    // Navigation
    public Person Person { get; set; } = null!;
    public StaffRole StaffRole { get; set; } = null!;
    public Airline? Airline { get; set; }
    public Airport? Airport { get; set; }
    public ICollection<StaffAvailability> Availabilities { get; set; } = [];
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
    public ICollection<CheckIn> CheckIns { get; set; } = [];
}
