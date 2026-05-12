using System;
using Domain.Common;
using Domain.Entities.Location;
using Domain.ValueObjects.People;

namespace Domain.Entities.People;

public sealed class Person : BaseEntity<int>
{
    public int DocumentTypeId { get; private set; }
    public DocumentNumber DocumentNumber { get; private set; } = null!;
    public PersonName FirstName { get; private set; } = null!;
    public PersonName LastName { get; private set; } = null!;
    public DateOnly? BirthDate { get; private set; }
    public Gender? Gender { get; private set; }
    public int? AddressId { get; private set; }

    public DocumentType DocumentType { get; set; } = null!;
    public Address? Address { get; set; }
    public ICollection<PersonEmail> Emails { get; set; } = [];
    public ICollection<PersonPhone> Phones { get; set; } = [];

    private Person() { }

    public Person(
        int documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        DateOnly? birthDate,
        string? gender,
        int? addressId)
    {
        DocumentTypeId = documentTypeId;
        DocumentNumber = DocumentNumber.Create(documentNumber);
        FirstName = PersonName.Create(firstName);
        LastName = PersonName.Create(lastName);
        BirthDate = birthDate;
        Gender = Gender.Create(gender);
        AddressId = addressId;
    }

    public void Update(
        int documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        DateOnly? birthDate,
        string? gender,
        int? addressId)
    {
        DocumentTypeId = documentTypeId;
        DocumentNumber = DocumentNumber.Create(documentNumber);
        FirstName = PersonName.Create(firstName);
        LastName = PersonName.Create(lastName);
        BirthDate = birthDate;
        Gender = Gender.Create(gender);
        AddressId = addressId;
        UpdatedAt = DateTime.UtcNow;
    }
}
