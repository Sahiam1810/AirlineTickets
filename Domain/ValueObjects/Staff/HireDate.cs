namespace Domain.ValueObjects.Staff;

public sealed record HireDate
{
    public DateOnly Value { get; }

    private HireDate(DateOnly value)
    {
        Value = value;
    }

    public static HireDate Create(DateOnly value)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (value > today)
            throw new ArgumentException("Hire date cannot be in the future", nameof(value));

        return new HireDate(value);
    }

    public override string ToString() => Value.ToString("yyyy-MM-dd");
}
