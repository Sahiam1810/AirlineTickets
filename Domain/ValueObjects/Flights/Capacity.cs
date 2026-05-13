namespace Domain.ValueObjects.Flights;

public sealed record Capacity
{
    public int Value { get; }

    private Capacity(int value)
    {
        Value = value;
    }

    public static Capacity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La capacidad total debe ser mayor que 0", nameof(value));

        return new Capacity(value);
    }

    public override string ToString() => Value.ToString();
}
