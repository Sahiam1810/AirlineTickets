namespace Domain.ValueObjects.Staff;

public sealed record StaffAvailabilityNotes
{
    public string Value { get; }

    private StaffAvailabilityNotes(string value)
    {
        Value = value;
    }

    public static StaffAvailabilityNotes? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 255)
            throw new ArgumentException("Staff availability notes cannot exceed 255 characters", nameof(value));

        return new StaffAvailabilityNotes(normalized);
    }

    public override string ToString() => Value;
}
