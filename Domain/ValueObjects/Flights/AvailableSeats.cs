namespace Domain.ValueObjects.Flights;

public sealed record AvailableSeats
{
    public int Value { get; }

    private AvailableSeats(int value)
    {
        Value = value;
    }

    public static AvailableSeats Create(int value, Capacity capacity)
    {
        if (value < 0)
            throw new ArgumentException("Los asientos disponibles no pueden ser negativos", nameof(value));

        if (value > capacity.Value)
            throw new ArgumentException("Los asientos disponibles no pueden superar la capacidad total", nameof(value));

        return new AvailableSeats(value);
    }

    public override string ToString() => Value.ToString();
}
