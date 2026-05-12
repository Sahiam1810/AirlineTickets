namespace Domain.ValueObjects.Staff;

public sealed record StaffRoleName
{
    public string Value { get; }

    private StaffRoleName(string value)
    {
        Value = value;
    }

    public static StaffRoleName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Staff role name is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("Staff role name cannot exceed 100 characters", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("Staff role name can only contain letters and spaces", nameof(value));

        return new StaffRoleName(normalized);
    }

    public override string ToString() => Value;
}
