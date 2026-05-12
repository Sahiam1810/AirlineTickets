namespace Api.Dtos.PhoneCodes;

public sealed class PhoneCodeDto
{
    public int Id { get; init; }
    public string CountryCode { get; init; } = default!;
    public string CountryName { get; init; } = default!;
}
