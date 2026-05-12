namespace Api.Dtos.PersonEmails;

public sealed class CreatePersonEmailRequest
{
    public int PersonId { get; init; }
    public string EmailUser { get; init; } = default!;
    public int EmailDomainId { get; init; }
    public bool IsPrimary { get; init; }
}
