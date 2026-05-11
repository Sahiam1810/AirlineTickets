using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class EmailDomain : BaseEntity<int>
{
    public string Domain { get; set; } = string.Empty;

    // Navigation
    public ICollection<PersonEmail> PersonEmails { get; set; } = [];
}