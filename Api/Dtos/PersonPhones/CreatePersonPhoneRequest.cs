namespace Api.Dtos.PersonPhones;

public sealed class CreatePersonPhoneRequest
{
    public int PersonId { get; init; }
    public int PhoneCodeId { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public bool IsPrimary { get; init; }
}
