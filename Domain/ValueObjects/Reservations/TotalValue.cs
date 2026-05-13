namespace Domain.ValueObjects.Reservations;

public sealed class TotalValue
{
    public decimal Value { get; }

    private TotalValue(decimal value)
    {
        Value = value;
    }

    public static TotalValue Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Total value cannot be negative.");

        return new TotalValue(value);
    }

    public override string ToString() => Value.ToString();
}
