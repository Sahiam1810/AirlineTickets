namespace Domain.ValueObjects.FlightAssignments;

public sealed record AssignmentKey
{
    public int FlightId { get; }
    public int StaffId { get; }

    private AssignmentKey(int flightId, int staffId)
    {
        FlightId = flightId;
        StaffId = staffId;
    }

    public static AssignmentKey Create(int flightId, int staffId)
    {
        if (flightId <= 0)
            throw new ArgumentException("Flight id is required", nameof(flightId));

        if (staffId <= 0)
            throw new ArgumentException("Staff id is required", nameof(staffId));

        return new AssignmentKey(flightId, staffId);
    }
}
