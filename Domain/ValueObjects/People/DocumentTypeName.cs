namespace Domain.ValueObjects.People;

public sealed record DocumentTypeName
{
    public string Value { get; }

    private DocumentTypeName(string value)
    {
        Value = value;
    }

    public static DocumentTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de documento es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(value));

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre solo puede contener letras y espacios", nameof(value));

        return new DocumentTypeName(normalized);
    }

    public override string ToString() => Value;
}
