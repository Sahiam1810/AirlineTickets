namespace Api.Dtos.People;

public sealed class PersonDto
{
    public int Id { get; init; }
    public int DocumentTypeId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public DateOnly? BirthDate { get; init; }
    public string? Gender { get; init; }
    public int? AddressId { get; init; }
}
