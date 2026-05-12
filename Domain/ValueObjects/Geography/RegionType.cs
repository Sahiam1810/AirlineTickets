namespace Domain.ValueObjects.Geography;

public sealed record RegionType
{
    public string Value { get; }

    private RegionType(string value)
    {
        Value = value;
    }

    public static RegionType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El tipo de region es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 30)
            throw new ArgumentException("El tipo no puede superar los 30 caracteres", nameof(value));

        return new RegionType(normalized);
    }

    public override string ToString() => Value;
}
