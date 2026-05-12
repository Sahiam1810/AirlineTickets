namespace Domain.ValueObjects.Location;

public sealed record AddressNumber
{
    public string Value { get; }

    private AddressNumber(string value)
    {
        Value = value;
    }

    public static AddressNumber? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new ArgumentException("El numero no puede superar los 20 caracteres", nameof(value));

        return new AddressNumber(normalized);
    }

    public override string ToString() => Value;
}
