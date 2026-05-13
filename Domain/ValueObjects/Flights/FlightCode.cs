namespace Domain.ValueObjects.Flights;

public sealed record FlightCode
{
    public string Value { get; }

    private FlightCode(string value)
    {
        Value = value;
    }

    public static FlightCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El codigo de vuelo es obligatorio", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 10)
            throw new ArgumentException("El codigo de vuelo no puede superar los 10 caracteres", nameof(value));

        if (!normalized.All(char.IsLetterOrDigit))
            throw new ArgumentException("El codigo de vuelo solo puede contener letras y numeros", nameof(value));

        return new FlightCode(normalized);
    }

    public override string ToString() => Value;
}
