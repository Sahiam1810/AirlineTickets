namespace Api.Dtos.Passengers;

public sealed class CreatePassengerRequest
{
    public int PersonId { get; init; }
    public int PassengerTypeId { get; init; }
}
