using System;
using Domain.Common;
using Domain.ValueObjects.People;

namespace Domain.Entities.People;

public sealed class DocumentType : BaseEntity<int>
{
    public DocumentTypeName Name { get; private set; } = null!;
    public DocumentTypeCode Code { get; private set; } = null!;

    public ICollection<Person> People { get; set; } = [];

    private DocumentType() { }

    public DocumentType(string name, string code)
    {
        Name = DocumentTypeName.Create(name);
        Code = DocumentTypeCode.Create(code);
    }

    public void Update(string name, string code)
    {
        Name = DocumentTypeName.Create(name);
        Code = DocumentTypeCode.Create(code);
        UpdatedAt = DateTime.UtcNow;
    }
}
