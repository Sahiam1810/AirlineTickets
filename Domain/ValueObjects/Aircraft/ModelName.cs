namespace Domain.ValueObjects.Aircraft;

public sealed record ModelName
{
    public string Value { get; }

    private ModelName(string value)
    {
        Value = value;
    }

    public static ModelName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Model name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("Model name cannot exceed 100 characters", nameof(value));

        return new ModelName(normalized);
    }

    public override string ToString() => Value;
}
