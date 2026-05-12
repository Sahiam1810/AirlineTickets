namespace Domain.ValueObjects.People;

public sealed record DocumentNumber
{
    public string Value { get; }

    private DocumentNumber(string value)
    {
        Value = value;
    }

    public static DocumentNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El numero de documento es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 30)
            throw new ArgumentException("El numero de documento no puede superar los 30 caracteres", nameof(value));

        return new DocumentNumber(normalized);
    }

    public override string ToString() => Value;
}
