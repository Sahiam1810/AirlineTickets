namespace Api.Dtos.PersonPhones;

public sealed class PersonPhoneDto
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public int PhoneCodeId { get; init; }
    public string CountryCode { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Phone { get; init; } = default!;
    public bool IsPrimary { get; init; }
}
