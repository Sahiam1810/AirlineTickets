namespace Api.Dtos.Staff;

public sealed class CreateStaffRequest
{
    public int PersonId { get; init; }
    public int StaffRoleId { get; init; }
    public int? AirlineId { get; init; }
    public int? AirportId { get; init; }
    public DateOnly HireDate { get; init; }
    public bool IsActive { get; init; }
}
