using System;
using Domain.Common;
using Domain.ValueObjects.EmailDomains;

namespace Domain.Entities.People;

public sealed class EmailDomain : BaseEntity<int>
{
    public EmailDomainValue Domain { get; private set; } = null!;

    private EmailDomain() { }

    public EmailDomain(EmailDomainValue domain)
    {
        Domain = domain;
    }

    public void Update(EmailDomainValue domain)
    {
        Domain = domain;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<PersonEmail> PersonEmails { get; set; } = [];
}
