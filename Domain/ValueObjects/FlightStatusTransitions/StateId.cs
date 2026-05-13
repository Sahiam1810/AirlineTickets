namespace Domain.ValueObjects.FlightStatusTransitions;

public sealed record StateId
{
    public int Value { get; }

    private StateId(int value)
    {
        Value = value;
    }

    public static StateId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El estado es obligatorio", nameof(value));

        return new StateId(value);
    }

    public override string ToString() => Value.ToString();
}
