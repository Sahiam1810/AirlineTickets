using System;
using Domain.Common;
using Domain.ValueObjects.Payments;

namespace Domain.Entities.Payments;

public sealed class InvoiceItemType : BaseEntity<int>
{
    public InvoiceItemTypeName Name { get; private set; } = null!;

    private InvoiceItemType() { }

    public InvoiceItemType(string name)
    {
        Name = InvoiceItemTypeName.Create(name);
    }

    public void Update(string name)
    {
        Name = InvoiceItemTypeName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }
}
