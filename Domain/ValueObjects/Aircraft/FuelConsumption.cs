namespace Domain.ValueObjects.Aircraft;

public sealed record FuelConsumption
{
    public decimal Value { get; }

    private FuelConsumption(decimal value)
    {
        Value = value;
    }

    public static FuelConsumption? Create(decimal? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Fuel consumption must be greater than 0", nameof(value));

        return new FuelConsumption(value.Value);
    }

    public override string ToString() => Value.ToString();
}
