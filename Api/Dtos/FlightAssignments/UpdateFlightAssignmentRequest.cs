namespace Api.Dtos.FlightAssignments;

public sealed class UpdateFlightAssignmentRequest
{
    public int FlightId { get; init; }
    public int StaffId { get; init; }
    public int FlightRoleId { get; init; }
}
