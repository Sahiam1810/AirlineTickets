namespace Api.Dtos.StaffAvailabilities;

public sealed class UpdateStaffAvailabilityRequest
{
    public int AvailabilityStatusId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? Notes { get; init; }
}
