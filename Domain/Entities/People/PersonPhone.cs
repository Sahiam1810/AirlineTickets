using System;

namespace Domain.Entities.People;

public sealed class PersonPhone
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PhoneCodeId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
    public PhoneCode PhoneCode { get; set; } = null!;
}