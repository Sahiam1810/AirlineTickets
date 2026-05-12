namespace Api.Dtos.Clients;

public sealed class ClientDto
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
}
