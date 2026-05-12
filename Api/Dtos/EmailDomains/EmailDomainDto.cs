namespace Api.Dtos.EmailDomains;

public sealed class EmailDomainDto
{
    public int Id { get; init; }
    public string Domain { get; init; } = default!;
}
