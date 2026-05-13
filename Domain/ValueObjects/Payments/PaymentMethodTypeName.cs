namespace Domain.ValueObjects.Payments;


public sealed record PaymentMethodTypeName
{
    public string Value { get; }
    private PaymentMethodTypeName(string value) => Value = value;

    public static PaymentMethodTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Payment method type name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 50)
            throw new ArgumentException("Payment method type name cannot exceed 50 characters", nameof(value));
        return new PaymentMethodTypeName(normalized);
    }

    public override string ToString() => Value;
}