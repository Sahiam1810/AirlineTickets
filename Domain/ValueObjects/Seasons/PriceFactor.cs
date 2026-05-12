namespace Domain.ValueObjects.Seasons;

public sealed record PriceFactor
{
    public decimal Value { get; }

    private PriceFactor(decimal value)
    {
        Value = value;
    }

    public static PriceFactor Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("El factor de precio debe ser mayor que 0", nameof(value));

        return new PriceFactor(value);
    }

    public override string ToString() => Value.ToString();
}
