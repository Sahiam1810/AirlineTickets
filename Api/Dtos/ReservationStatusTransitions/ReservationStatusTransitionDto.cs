namespace Api.Dtos.ReservationStatusTransitions;

public sealed class ReservationStatusTransitionDto
{
    public int Id { get; init; }
    public int FromStatusId { get; init; }
    public string FromStatusName { get; init; } = default!;
    public int ToStatusId { get; init; }
    public string ToStatusName { get; init; } = default!;
}
