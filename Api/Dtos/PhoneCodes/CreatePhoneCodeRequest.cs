namespace Api.Dtos.PhoneCodes;

public sealed class CreatePhoneCodeRequest
{
    public string CountryCode { get; init; } = default!;
    public string CountryName { get; init; } = default!;
}
