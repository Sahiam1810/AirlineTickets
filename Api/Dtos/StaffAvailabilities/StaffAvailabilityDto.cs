namespace Api.Dtos.StaffAvailabilities;

public sealed class StaffAvailabilityDto
{
    public int Id { get; init; }
    public int StaffId { get; init; }
    public string StaffName { get; init; } = default!;
    public int AvailabilityStatusId { get; init; }
    public string AvailabilityStatusName { get; init; } = default!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? Notes { get; init; }
}
