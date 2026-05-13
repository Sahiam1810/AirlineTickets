namespace Api.Dtos.Passengers;

public sealed class PassengerDto
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public int PassengerTypeId { get; init; }
    public string PassengerTypeName { get; init; } = default!;
}
