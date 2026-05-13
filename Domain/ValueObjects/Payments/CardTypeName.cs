namespace Domain.ValueObjects.Payments;

public sealed record CardTypeName
{
    public string Value { get; }
    private CardTypeName(string value) => Value = value;

    public static CardTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Card type name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 50)
            throw new ArgumentException("Card type name cannot exceed 50 characters", nameof(value));
        return new CardTypeName(normalized);
    }

    public override string ToString() => Value;
}