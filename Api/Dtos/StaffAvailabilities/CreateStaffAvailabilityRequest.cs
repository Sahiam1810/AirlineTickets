namespace Api.Dtos.StaffAvailabilities;

public sealed class CreateStaffAvailabilityRequest
{
    public int StaffId { get; init; }
    public int AvailabilityStatusId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? Notes { get; init; }
}
