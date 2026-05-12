namespace Domain.ValueObjects.Airports;

public sealed record IcaoCode
{
    public string Value { get; }

    private IcaoCode(string value)
    {
        Value = value;
    }

    public static IcaoCode? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length != 4)
            throw new ArgumentException("ICAO code must have exactly 4 characters", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("ICAO code can only contain letters", nameof(value));

        return new IcaoCode(normalized);
    }

    public override string ToString() => Value;
}
