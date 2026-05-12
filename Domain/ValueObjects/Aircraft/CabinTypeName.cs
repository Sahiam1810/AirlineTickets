namespace Domain.ValueObjects.Aircraft;

public sealed record CabinTypeName
{
    public string Value { get; }

    private CabinTypeName(string value)
    {
        Value = value;
    }

    public static CabinTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Cabin type name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("Cabin type name cannot exceed 50 characters", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("Cabin type name can only contain letters and spaces", nameof(value));

        return new CabinTypeName(normalized);
    }

    public override string ToString() => Value;
}
