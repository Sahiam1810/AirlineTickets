namespace Api.Dtos.Staff;

public sealed class StaffDto
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public int StaffRoleId { get; init; }
    public string StaffRoleName { get; init; } = default!;
    public int? AirlineId { get; init; }
    public string? AirlineName { get; init; }
    public int? AirportId { get; init; }
    public string? AirportName { get; init; }
    public DateOnly HireDate { get; init; }
    public bool IsActive { get; init; }
}
