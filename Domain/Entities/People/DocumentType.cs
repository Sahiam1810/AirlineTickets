using System;

namespace Domain.Entities.People;

public sealed class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    // Navigation
    public ICollection<Person> People { get; set; } = [];
}