using System;

namespace Domain.Entities.People;

public sealed class EmailDomain
{
    public int Id { get; set; }
    public string Domain { get; set; } = string.Empty;

    // Navigation
    public ICollection<PersonEmail> PersonEmails { get; set; } = [];
}