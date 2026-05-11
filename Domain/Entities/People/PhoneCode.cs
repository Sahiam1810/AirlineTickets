using System;

namespace Domain.Entities.People;

public sealed class PhoneCode
{
    public int Id { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;

    // Navigation
    public ICollection<PersonPhone> PersonPhones { get; set; } = [];
}