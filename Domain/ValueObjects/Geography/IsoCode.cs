namespace Domain.ValueObjects.Geography;

public sealed record IsoCode
{
    public string Value { get; }
    private IsoCode(string value) => Value = value;

    public static IsoCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ISO code is required", nameof(value));
        var normalized = value.Trim().ToUpper();
        if (normalized.Length > 3)
            throw new ArgumentException("ISO code cannot exceed 3 characters", nameof(value));
        return new IsoCode(normalized);
    }

    public override string ToString() => Value;
}