namespace Domain.ValueObjects.Airports;

public sealed record IataCode
{
    public string Value { get; }

    private IataCode(string value)
    {
        Value = value;
    }

    public static IataCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("IATA code is required", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length != 3)
            throw new ArgumentException("IATA code must have exactly 3 characters", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("IATA code can only contain letters", nameof(value));

        return new IataCode(normalized);
    }

    public override string ToString() => Value;
}
