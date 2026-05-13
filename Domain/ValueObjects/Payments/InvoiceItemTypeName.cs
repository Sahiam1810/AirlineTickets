using System;

namespace Domain.ValueObjects.Payments;

public sealed class InvoiceItemTypeName
{
    public string Value { get; }

    private InvoiceItemTypeName(string value)
    {
        Value = value;
    }

    public static InvoiceItemTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice item type name cannot be null or empty.");

        if (value.Length > 100)
            throw new ArgumentException("Invoice item type name cannot exceed 100 characters.");

        return new InvoiceItemTypeName(value.Trim());
    }

    public override string ToString() => Value;
}
