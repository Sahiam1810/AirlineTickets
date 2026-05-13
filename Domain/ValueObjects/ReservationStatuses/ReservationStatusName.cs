namespace Domain.ValueObjects.ReservationStatuses;

public sealed record ReservationStatusName
{
    public string Value { get; }

    private ReservationStatusName(string value)
    {
        Value = value;
    }

    public static ReservationStatusName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de reserva es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre del estado de reserva no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("El nombre del estado de reserva solo puede contener letras", nameof(value));

        return new ReservationStatusName(normalized);
    }

    public override string ToString() => Value;
}
