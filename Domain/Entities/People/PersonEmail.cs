using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class PersonEmail : BaseEntity<int>
{
    public int PersonId { get; set; }
    public string EmailUser { get; set; } = string.Empty;
    public int EmailDomainId { get; set; }
    public bool IsPrimary { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
    public EmailDomain EmailDomain { get; set; } = null!;
}