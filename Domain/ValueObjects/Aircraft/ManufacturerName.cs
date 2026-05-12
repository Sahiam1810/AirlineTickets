namespace Domain.ValueObjects.Aircraft;

public sealed record ManufacturerName
{
    public string Value { get; }

    private ManufacturerName(string value)
    {
        Value = value;
    }

    public static ManufacturerName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Manufacturer name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("Manufacturer name cannot exceed 100 characters", nameof(value));

        return new ManufacturerName(normalized);
    }

    public override string ToString() => Value;
}
