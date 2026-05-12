namespace Domain.ValueObjects.Location;

public sealed record AddressComplement
{
    public string Value { get; }

    private AddressComplement(string value)
    {
        Value = value;
    }

    public static AddressComplement? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 100)
            throw new ArgumentException("El complemento no puede superar los 100 caracteres", nameof(value));

        return new AddressComplement(normalized);
    }

    public override string ToString() => Value;
}
