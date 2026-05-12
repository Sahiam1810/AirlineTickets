namespace Domain.ValueObjects.Routes;

public sealed record DistanceKm
{
    public int Value { get; }

    private DistanceKm(int value)
    {
        Value = value;
    }

    public static DistanceKm? Create(int? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value <= 0)
            throw new ArgumentException("Distance must be greater than 0", nameof(value));

        return new DistanceKm(value.Value);
    }

    public override string ToString() => Value.ToString();
}
