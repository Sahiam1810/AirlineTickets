namespace Api.Dtos.EmailDomains;

public sealed class UpdateEmailDomainRequest
{
    public string Domain { get; init; } = default!;
}
