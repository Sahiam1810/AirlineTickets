namespace Domain.ValueObjects.SeatLocationTypes;

public sealed record SeatLocationTypeName
{
    public string Value { get; }

    private SeatLocationTypeName(string value)
    {
        Value = value;
    }

    public static SeatLocationTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de ubicacion del asiento es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre del tipo de ubicacion del asiento no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("El nombre del tipo de ubicacion del asiento solo puede contener letras", nameof(value));

        return new SeatLocationTypeName(normalized);
    }

    public override string ToString() => Value;
}
