namespace Domain.ValueObjects.AirportAirlines;

public sealed record Terminal
{
    public string Value { get; }

    private Terminal(string value)
    {
        Value = value;
    }

    public static Terminal? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new ArgumentException("Terminal cannot exceed 20 characters", nameof(value));

        return new Terminal(normalized);
    }

    public override string ToString() => Value;
}
