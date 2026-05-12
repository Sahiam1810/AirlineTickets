namespace Domain.ValueObjects.Geography;
public sealed record RegionName
{
    public string Value { get; }
    private RegionName(string value) => Value = value;

    public static RegionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Region name is required", nameof(value));
        var normalized = value.Trim();
        if (normalized.Length > 100)
            throw new ArgumentException("Region name cannot exceed 100 characters", nameof(value));
        return new RegionName(normalized);
    }

    public override string ToString() => Value;
}