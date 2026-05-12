namespace Domain.ValueObjects.PersonPhones;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new ArgumentException("Phone number cannot exceed 20 characters", nameof(value));

        if (!normalized.All(char.IsDigit))
            throw new ArgumentException("Phone number can only contain digits", nameof(value));

        return new PhoneNumber(normalized);
    }

    public override string ToString() => Value;
}
