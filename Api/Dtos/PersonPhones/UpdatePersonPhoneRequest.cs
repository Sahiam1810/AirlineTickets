namespace Api.Dtos.PersonPhones;

public sealed class UpdatePersonPhoneRequest
{
    public int PhoneCodeId { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public bool IsPrimary { get; init; }
}
