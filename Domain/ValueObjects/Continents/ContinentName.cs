namespace Domain.ValueObjects.Continents;

public sealed record ContinentName
{
    public string Value { get; }

    private ContinentName(string value)
    {
        Value = value;
    }

    public static ContinentName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del continente es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(value));

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre no puede superar los 100 caracteres", nameof(value));

        // Opcional: validar solo letras y espacios
        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre solo puede contener letras y espacios", nameof(value));

        return new ContinentName(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator ContinentName(string v)
    {
        throw new NotImplementedException();
    }
}