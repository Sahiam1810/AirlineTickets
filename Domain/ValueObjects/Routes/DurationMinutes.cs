namespace Domain.ValueObjects.Routes;

public sealed record DurationMinutes
{
    public int Value { get; }

    private DurationMinutes(int value)
    {
        Value = value;
    }

    public static DurationMinutes? Create(int? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(value));

        return new DurationMinutes(value.Value);
    }

    public override string ToString() => Value.ToString();
}
