namespace Domain.ValueObjects.Aircraft;

public sealed record SpeedKmh
{
    public int Value { get; }

    private SpeedKmh(int value)
    {
        Value = value;
    }

    public static SpeedKmh? Create(int? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Speed must be greater than 0", nameof(value));

        return new SpeedKmh(value.Value);
    }

    public override string ToString() => Value.ToString();
}
