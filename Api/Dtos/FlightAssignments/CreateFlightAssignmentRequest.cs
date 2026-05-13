namespace Api.Dtos.FlightAssignments;

public sealed class CreateFlightAssignmentRequest
{
    public int FlightId { get; init; }
    public int StaffId { get; init; }
    public int FlightRoleId { get; init; }
}
