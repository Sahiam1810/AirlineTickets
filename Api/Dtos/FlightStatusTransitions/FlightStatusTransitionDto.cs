namespace Api.Dtos.FlightStatusTransitions;

public sealed class FlightStatusTransitionDto
{
    public int Id { get; init; }
    public int FromStateId { get; init; }
    public string FromStateName { get; init; } = default!;
    public int ToStateId { get; init; }
    public string ToStateName { get; init; } = default!;
}
