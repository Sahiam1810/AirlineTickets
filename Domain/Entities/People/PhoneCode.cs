using System;
using Domain.Common;
using Domain.ValueObjects.PhoneCodes;

namespace Domain.Entities.People;

public sealed class PhoneCode : BaseEntity<int>
{
    public CountryCode CountryCode { get; private set; } = null!;
    public CountryName CountryName { get; private set; } = null!;

    private PhoneCode() { }

    public PhoneCode(CountryCode countryCode, CountryName countryName)
    {
        CountryCode = countryCode;
        CountryName = countryName;
    }

    public void Update(CountryCode countryCode, CountryName countryName)
    {
        CountryCode = countryCode;
        CountryName = countryName;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<PersonPhone> PersonPhones { get; set; } = [];
}
