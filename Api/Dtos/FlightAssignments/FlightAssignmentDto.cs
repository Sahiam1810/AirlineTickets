namespace Api.Dtos.FlightAssignments;

public sealed class FlightAssignmentDto
{
    public int Id { get; init; }
    public int FlightId { get; init; }
    public string FlightCode { get; init; } = default!;
    public int StaffId { get; init; }
    public int PersonId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public int FlightRoleId { get; init; }
    public string FlightRoleName { get; init; } = default!;
}
