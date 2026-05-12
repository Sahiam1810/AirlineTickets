namespace Domain.ValueObjects.Geography;

public sealed record CountryName
{
    public string Value { get; }

    private CountryName(string value)
    {
        Value = value;
    }

    public static CountryName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del pais es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(value));

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre no puede superar los 100 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre solo puede contener letras y espacios", nameof(value));

        return new CountryName(normalized);
    }

    public override string ToString() => Value;
}
