using System;
using Domain.Common;
using Domain.Entities.Airlines;
using Domain.Entities.People;

namespace Domain.Entities.Staff;

public sealed class StaffMember : BaseEntity<int>
{
    public int PersonId { get; set; }
    public int RoleId { get; set; }
    public int? AirlineId { get; set; }
    public int? AirportId { get; set; }
    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
    public StaffRole Role { get; set; } = null!;
    public Airline? Airline { get; set; }
    public Airport? Airport { get; set; }
    public ICollection<StaffAvailability> Availabilities { get; set; } = [];
}