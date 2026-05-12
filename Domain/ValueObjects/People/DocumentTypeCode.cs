namespace Domain.ValueObjects.People;

public sealed record DocumentTypeCode
{
    public string Value { get; }

    private DocumentTypeCode(string value)
    {
        Value = value;
    }

    public static DocumentTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El codigo del tipo de documento es obligatorio", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 10)
            throw new ArgumentException("El codigo no puede superar los 10 caracteres", nameof(value));

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("El codigo solo puede contener letras", nameof(value));

        return new DocumentTypeCode(normalized);
    }

    public override string ToString() => Value;
}
