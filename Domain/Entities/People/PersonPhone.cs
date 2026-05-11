using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class PersonPhone : BaseEntity<int>
{
    public int PersonId { get; set; }
    public int PhoneCodeId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
    public PhoneCode PhoneCode { get; set; } = null!;
}