using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class PhoneCode : BaseEntity<int>
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;

    // Navigation
    public ICollection<PersonPhone> PersonPhones { get; set; } = [];
}