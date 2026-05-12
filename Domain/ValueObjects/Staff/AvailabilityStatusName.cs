namespace Domain.ValueObjects.Staff;

public sealed record AvailabilityStatusName
{
    public string Value { get; }

    private AvailabilityStatusName(string value)
    {
        Value = value;
    }

    public static AvailabilityStatusName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Availability status name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("Availability status name cannot exceed 50 characters", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("Availability status name can only contain letters and spaces", nameof(value));

        return new AvailabilityStatusName(normalized);
    }

    public override string ToString() => Value;
}
