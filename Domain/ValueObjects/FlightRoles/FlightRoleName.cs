namespace Domain.ValueObjects.FlightRoles;

public sealed record FlightRoleName
{
    public string Value { get; }

    private FlightRoleName(string value)
    {
        Value = value;
    }

    public static FlightRoleName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del rol de vuelo es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre del rol de vuelo no puede superar los 100 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre del rol de vuelo solo puede contener letras y espacios", nameof(value));

        return new FlightRoleName(normalized);
    }

    public override string ToString() => Value;
}
