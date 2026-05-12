namespace Api.Dtos.PersonEmails;

public sealed class PersonEmailDto
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public string EmailUser { get; init; } = default!;
    public int EmailDomainId { get; init; }
    public string Domain { get; init; } = default!;
    public string Email { get; init; } = default!;
    public bool IsPrimary { get; init; }
}
