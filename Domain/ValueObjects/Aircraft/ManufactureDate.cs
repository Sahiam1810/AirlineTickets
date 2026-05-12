namespace Domain.ValueObjects.Aircraft;

public sealed record ManufactureDate
{
    public DateOnly Value { get; }

    private ManufactureDate(DateOnly value)
    {
        Value = value;
    }

    public static ManufactureDate? Create(DateOnly? value)
    {
        if (!value.HasValue)
            return null;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (value.Value > today)
            throw new ArgumentException("Manufacture date cannot be in the future", nameof(value));

        return new ManufactureDate(value.Value);
    }

    public override string ToString() => Value.ToString();
}
