namespace Domain.ValueObjects.PassengerTypes;

public sealed record Age
{
    public int Value { get; }

    private Age(int value)
    {
        Value = value;
    }

    public static Age? Create(int? value)
    {
        if (value is null)
            return null;

        if (value < 0)
            throw new ArgumentException("La edad no puede ser negativa", nameof(value));

        return new Age(value.Value);
    }

    public override string ToString() => Value.ToString();
}
