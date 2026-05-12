namespace Domain.ValueObjects.Aircraft;

public sealed record RowNumber
{
    public int Value { get; }

    private RowNumber(int value)
    {
        Value = value;
    }

    public static RowNumber Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Row number must be greater than 0", nameof(value));

        return new RowNumber(value);
    }

    public override string ToString() => Value.ToString();
}
