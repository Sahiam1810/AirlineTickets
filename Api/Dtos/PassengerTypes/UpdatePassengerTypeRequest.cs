namespace Api.Dtos.PassengerTypes;

public sealed class UpdatePassengerTypeRequest
{
    public string Name { get; init; } = default!;
    public int? AgeMin { get; init; }
    public int? AgeMax { get; init; }
}
