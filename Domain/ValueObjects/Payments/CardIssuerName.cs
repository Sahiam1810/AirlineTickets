namespace Domain.ValueObjects.Payments;
public sealed record CardIssuerName
{
    public string Value { get; }
    private CardIssuerName(string value) => Value = value;

    public static CardIssuerName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Card issuer name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 50)
            throw new ArgumentException("Card issuer name cannot exceed 50 characters", nameof(value));
        return new CardIssuerName(normalized);
    }

    public override string ToString() => Value;
}