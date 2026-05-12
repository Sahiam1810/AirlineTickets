using System;
using Domain.Common;
using Domain.ValueObjects.PersonPhones;

namespace Domain.Entities.People;

public sealed class PersonPhone : BaseEntity<int>
{
    public int PersonId { get; private set; }
    public int PhoneCodeId { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    private PersonPhone() { }

    public PersonPhone(int personId, int phoneCodeId, PhoneNumber phoneNumber, bool isPrimary)
    {
        if (personId <= 0)
            throw new ArgumentException("Person id is required", nameof(personId));

        if (phoneCodeId <= 0)
            throw new ArgumentException("Phone code id is required", nameof(phoneCodeId));

        PersonId = personId;
        PhoneCodeId = phoneCodeId;
        PhoneNumber = phoneNumber;
        IsPrimary = isPrimary;
    }

    public void Update(int phoneCodeId, PhoneNumber phoneNumber, bool isPrimary)
    {
        if (phoneCodeId <= 0)
            throw new ArgumentException("Phone code id is required", nameof(phoneCodeId));

        PhoneCodeId = phoneCodeId;
        PhoneNumber = phoneNumber;
        IsPrimary = isPrimary;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public Person Person { get; set; } = null!;
    public PhoneCode PhoneCode { get; set; } = null!;
}
