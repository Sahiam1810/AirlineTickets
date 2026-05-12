namespace Domain.ValueObjects.Aircraft;

public sealed record WeightKg
{
    public decimal Value { get; }

    private WeightKg(decimal value)
    {
        Value = value;
    }

    public static WeightKg? Create(decimal? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Weight must be greater than 0", nameof(value));

        return new WeightKg(value.Value);
    }

    public override string ToString() => Value.ToString();
}
