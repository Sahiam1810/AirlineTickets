namespace Api.Dtos.CheckInStatuses;

public sealed class CheckInStatusDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
