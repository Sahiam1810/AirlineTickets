namespace Domain.ValueObjects.FlightStates;

public sealed record FlightStateName
{
    public string Value { get; }

    private FlightStateName(string value)
    {
        Value = value;
    }

    public static FlightStateName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de vuelo es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre del estado de vuelo no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre del estado de vuelo solo puede contener letras y espacios", nameof(value));

        return new FlightStateName(normalized);
    }

    public override string ToString() => Value;
}
