namespace Domain.ValueObjects.Airports;

public sealed record AirportName
{
    public string Value { get; }

    private AirportName(string value)
    {
        Value = value;
    }

    public static AirportName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Airport name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 150)
            throw new ArgumentException("Airport name cannot exceed 150 characters", nameof(value));

        return new AirportName(normalized);
    }

    public override string ToString() => Value;
}
