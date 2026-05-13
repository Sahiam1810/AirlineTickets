namespace Api.Dtos.FlightStatusTransitions;

public sealed class CreateFlightStatusTransitionRequest
{
    public int FromStateId { get; init; }
    public int ToStateId { get; init; }
}
