namespace Api.Dtos.TicketStatuses;

public sealed class TicketStatusDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
