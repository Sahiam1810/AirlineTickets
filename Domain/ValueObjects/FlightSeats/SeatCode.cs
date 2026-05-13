namespace Domain.ValueObjects.FlightSeats;

public sealed record SeatCode
{
    public string Value { get; }

    private SeatCode(string value)
    {
        Value = value;
    }

    public static SeatCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El codigo del asiento es obligatorio", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 5)
            throw new ArgumentException("El codigo del asiento no puede superar los 5 caracteres", nameof(value));

        if (normalized.Length < 2)
            throw new ArgumentException("El codigo del asiento debe tener numero y letra", nameof(value));

        var row = normalized[..^1];
        var location = normalized[^1];

        if (!row.All(char.IsDigit) || !char.IsLetter(location))
            throw new ArgumentException("El codigo del asiento debe tener formato numero + letra, por ejemplo 12A", nameof(value));

        return new SeatCode(normalized);
    }

    public override string ToString() => Value;
}
