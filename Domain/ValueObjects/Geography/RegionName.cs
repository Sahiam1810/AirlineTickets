namespace Domain.ValueObjects.Geography;

public sealed record RegionName
{
    public string Value { get; }

    private RegionName(string value)
    {
        Value = value;
    }

    public static RegionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la region es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(value));

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre no puede superar los 100 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre solo puede contener letras y espacios", nameof(value));

        return new RegionName(normalized);
    }

    public override string ToString() => Value;
}
