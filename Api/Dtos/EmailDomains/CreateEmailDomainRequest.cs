namespace Api.Dtos.EmailDomains;

public sealed class CreateEmailDomainRequest
{
    public string Domain { get; init; } = default!;
}
