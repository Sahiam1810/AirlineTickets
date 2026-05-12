namespace Domain.ValueObjects.Geography;

public sealed record CountryName
{
    public string Value { get; }
    private CountryName(string value) => Value = value;

    public static CountryName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Country name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 100)
            throw new ArgumentException("Country name cannot exceed 100 characters", nameof(value));
        return new CountryName(normalized);
    }

    public override string ToString() => Value;
}