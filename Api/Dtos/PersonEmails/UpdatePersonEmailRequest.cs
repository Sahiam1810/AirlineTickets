namespace Api.Dtos.PersonEmails;

public sealed class UpdatePersonEmailRequest
{
    public string EmailUser { get; init; } = default!;
    public int EmailDomainId { get; init; }
    public bool IsPrimary { get; init; }
}
