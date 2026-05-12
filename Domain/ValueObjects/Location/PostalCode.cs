namespace Domain.ValueObjects.Location;

public sealed record PostalCode
{
    public string Value { get; }

    private PostalCode(string value)
    {
        Value = value;
    }

    public static PostalCode? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new ArgumentException("El codigo postal no puede superar los 20 caracteres", nameof(value));

        return new PostalCode(normalized);
    }

    public override string ToString() => Value;
}
