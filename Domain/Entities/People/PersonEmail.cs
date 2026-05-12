using System;
using Domain.Common;
using Domain.ValueObjects.PersonEmails;

namespace Domain.Entities.People;

public sealed class PersonEmail : BaseEntity<int>
{
    public int PersonId { get; private set; }
    public EmailUser EmailUser { get; private set; } = null!;
    public int EmailDomainId { get; private set; }
    public bool IsPrimary { get; private set; }

    private PersonEmail() { }

    public PersonEmail(int personId, EmailUser emailUser, int emailDomainId, bool isPrimary)
    {
        if (personId <= 0)
            throw new ArgumentException("Person id is required", nameof(personId));

        if (emailDomainId <= 0)
            throw new ArgumentException("Email domain id is required", nameof(emailDomainId));

        PersonId = personId;
        EmailUser = emailUser;
        EmailDomainId = emailDomainId;
        IsPrimary = isPrimary;
    }

    public void Update(EmailUser emailUser, int emailDomainId, bool isPrimary)
    {
        if (emailDomainId <= 0)
            throw new ArgumentException("Email domain id is required", nameof(emailDomainId));

        EmailUser = emailUser;
        EmailDomainId = emailDomainId;
        IsPrimary = isPrimary;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public Person Person { get; set; } = null!;
    public EmailDomain EmailDomain { get; set; } = null!;
}
