using System;

namespace Domain.ValueObjects.Payments;

public sealed record PaymentStateName
{
    public string Value { get; }
    private PaymentStateName(string value) => Value = value;

    public static PaymentStateName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Payment state name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 50)
            throw new ArgumentException("Payment state name cannot exceed 50 characters", nameof(value));
        return new PaymentStateName(normalized);
    }

    public override string ToString() => Value;
}