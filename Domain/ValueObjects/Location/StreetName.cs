namespace Domain.ValueObjects.Location;

public sealed record StreetName
{
    public string Value { get; }

    private StreetName(string value)
    {
        Value = value;
    }

    public static StreetName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la via es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre de la via no puede superar los 100 caracteres", nameof(value));

        return new StreetName(normalized);
    }

    public override string ToString() => Value;
}
