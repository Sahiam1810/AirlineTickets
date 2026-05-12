namespace Domain.ValueObjects.Aircraft;

public sealed record MaxCapacity
{
    public int Value { get; }

    private MaxCapacity(int value)
    {
        Value = value;
    }

    public static MaxCapacity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Max capacity must be greater than 0", nameof(value));

        return new MaxCapacity(value);
    }

    public override string ToString() => Value.ToString();
}
