namespace Domain.ValueObjects.Geography;

public sealed record CityName
{
    public string Value { get; }
    private CityName(string value) => Value = value;

    public static CityName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("City name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 100)
            throw new ArgumentException("City name cannot exceed 100 characters", nameof(value));
        return new CityName(normalized);
    }

    public override string ToString() => Value;
}