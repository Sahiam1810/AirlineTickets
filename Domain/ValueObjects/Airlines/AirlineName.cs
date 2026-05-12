namespace Domain.ValueObjects.Airlines;

public sealed record AirlineName
{
    public string Value { get; }

    private AirlineName(string value)
    {
        Value = value;
    }

    public static AirlineName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Airline name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 150)
            throw new ArgumentException("Airline name cannot exceed 150 characters", nameof(value));

        return new AirlineName(normalized);
    }

    public override string ToString() => Value;
}
