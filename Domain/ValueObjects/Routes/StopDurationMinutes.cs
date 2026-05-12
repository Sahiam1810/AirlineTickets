namespace Domain.ValueObjects.Routes;

public sealed record StopDurationMinutes
{
    public int Value { get; }

    private StopDurationMinutes(int value)
    {
        Value = value;
    }

    public static StopDurationMinutes Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("Stop duration must be greater than or equal to 0", nameof(value));

        return new StopDurationMinutes(value);
    }

    public override string ToString() => Value.ToString();
}
