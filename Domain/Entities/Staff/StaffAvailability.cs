using System;
using Domain.Common;
using Domain.ValueObjects.Staff;

namespace Domain.Entities.Staff;

public sealed class StaffAvailability : BaseEntity<int>
{
    public int StaffId { get; private set; }
    public int AvailabilityStatusId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public StaffAvailabilityNotes? Notes { get; private set; }

    private StaffAvailability() { }

    public StaffAvailability(int staffId, int availabilityStatusId, DateTime startDate, DateTime endDate, StaffAvailabilityNotes? notes)
    {
        Validate(staffId, availabilityStatusId, startDate, endDate);

        StaffId = staffId;
        AvailabilityStatusId = availabilityStatusId;
        StartDate = startDate;
        EndDate = endDate;
        Notes = notes;
    }

    public void Update(int availabilityStatusId, DateTime startDate, DateTime endDate, StaffAvailabilityNotes? notes)
    {
        Validate(StaffId, availabilityStatusId, startDate, endDate);

        AvailabilityStatusId = availabilityStatusId;
        StartDate = startDate;
        EndDate = endDate;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int staffId, int availabilityStatusId, DateTime startDate, DateTime endDate)
    {
        if (staffId <= 0)
            throw new ArgumentException("Staff id is required", nameof(staffId));

        if (availabilityStatusId <= 0)
            throw new ArgumentException("Availability status id is required", nameof(availabilityStatusId));

        _ = DateRange.Create(startDate, endDate);
    }

    // Navigation
    public StaffMember Staff { get; set; } = null!;
    public AvailabilityStatus AvailabilityStatus { get; set; } = null!;
}
