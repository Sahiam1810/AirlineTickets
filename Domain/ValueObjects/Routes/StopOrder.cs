namespace Domain.ValueObjects.Routes;

public sealed record StopOrder
{
    public int Value { get; }

    private StopOrder(int value)
    {
        Value = value;
    }

    public static StopOrder Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Stop order must be greater than 0", nameof(value));

        return new StopOrder(value);
    }

    public override string ToString() => Value.ToString();
}
