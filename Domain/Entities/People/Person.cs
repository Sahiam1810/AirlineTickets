using System;

namespace Domain.Entities.People;

public sealed class Person
{
    public int Id { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
    public char? Gender { get; set; }
    public int? AddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public DocumentType DocumentType { get; set; } = null!;
    public Location.Address? Address { get; set; }
    public ICollection<PersonEmail> Emails { get; set; } = [];
    public ICollection<PersonPhone> Phones { get; set; } = [];
}