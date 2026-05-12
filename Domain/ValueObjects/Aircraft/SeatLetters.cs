namespace Domain.ValueObjects.Aircraft;

public sealed record SeatLetters
{
    public string Value { get; }

    private SeatLetters(string value)
    {
        Value = value;
    }

    public static SeatLetters Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Seat letters are required", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 10)
            throw new ArgumentException("Seat letters cannot exceed 10 characters", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("Seat letters can only contain uppercase letters", nameof(value));

        return new SeatLetters(normalized);
    }

    public override string ToString() => Value;
}
