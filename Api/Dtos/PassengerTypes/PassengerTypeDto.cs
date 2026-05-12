namespace Api.Dtos.PassengerTypes;

public sealed class PassengerTypeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public int? AgeMin { get; init; }
    public int? AgeMax { get; init; }
}
