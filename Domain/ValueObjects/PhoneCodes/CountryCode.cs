namespace Domain.ValueObjects.PhoneCodes;

public sealed record CountryCode
{
    public string Value { get; }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static CountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Country code is required", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 5)
            throw new ArgumentException("Country code cannot exceed 5 characters", nameof(value));

        if (!normalized.StartsWith('+'))
            throw new ArgumentException("Country code must start with +", nameof(value));

        if (normalized.Length == 1 || !normalized[1..].All(char.IsDigit))
            throw new ArgumentException("Country code must contain only digits after +", nameof(value));

        return new CountryCode(normalized);
    }

    public override string ToString() => Value;
}
