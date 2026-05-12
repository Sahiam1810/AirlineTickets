namespace Domain.ValueObjects.Aircraft;

public sealed record AltitudeFt
{
    public int Value { get; }

    private AltitudeFt(int value)
    {
        Value = value;
    }

    public static AltitudeFt? Create(int? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Altitude must be greater than 0", nameof(value));

        return new AltitudeFt(value.Value);
    }

    public override string ToString() => Value.ToString();
}
