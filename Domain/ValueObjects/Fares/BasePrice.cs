namespace Domain.ValueObjects.Fares;

public sealed record BasePrice
{
    public decimal Value { get; }

    private BasePrice(decimal value)
    {
        Value = value;
    }

    public static BasePrice Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El precio base no puede ser negativo", nameof(value));

        return new BasePrice(value);
    }

    public override string ToString() => Value.ToString();
}
