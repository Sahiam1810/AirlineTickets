namespace Api.Dtos.ReservationStatusTransitions;

public sealed class CreateReservationStatusTransitionRequest
{
    public int FromStatusId { get; init; }
    public int ToStatusId { get; init; }
}
