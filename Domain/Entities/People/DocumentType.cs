using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class DocumentType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    // Navigation
    public ICollection<Person> People { get; set; } = [];
}