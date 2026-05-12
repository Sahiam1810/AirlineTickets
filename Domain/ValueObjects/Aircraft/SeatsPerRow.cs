namespace Domain.ValueObjects.Aircraft;

public sealed record SeatsPerRow
{
    public int Value { get; }

    private SeatsPerRow(int value)
    {
        Value = value;
    }

    public static SeatsPerRow Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Seats per row must be greater than 0", nameof(value));

        return new SeatsPerRow(value);
    }

    public override string ToString() => Value.ToString();
}
