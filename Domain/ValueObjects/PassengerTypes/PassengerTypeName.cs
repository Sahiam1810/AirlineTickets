namespace Domain.ValueObjects.PassengerTypes;

public sealed record PassengerTypeName
{
    public string Value { get; }

    private PassengerTypeName(string value)
    {
        Value = value;
    }

    public static PassengerTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de pasajero es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre del tipo de pasajero no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre del tipo de pasajero solo puede contener letras y espacios", nameof(value));

        return new PassengerTypeName(normalized);
    }

    public override string ToString() => Value;
}
