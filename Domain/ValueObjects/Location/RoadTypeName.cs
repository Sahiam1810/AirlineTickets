namespace Domain.ValueObjects.Location;

public sealed record RoadTypeName
{
    public string Value { get; }

    private RoadTypeName(string value)
    {
        Value = value;
    }

    public static RoadTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de via es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(value));

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre solo puede contener letras y espacios", nameof(value));

        return new RoadTypeName(normalized);
    }

    public override string ToString() => Value;
}
