namespace Domain.ValueObjects.People;

public sealed record Gender
{
    public string Value { get; }

    private Gender(string value)
    {
        Value = value;
    }

    public static Gender? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized is not ("M" or "F" or "N"))
            throw new ArgumentException("El genero debe ser M, F o N", nameof(value));

        return new Gender(normalized);
    }

    public override string ToString() => Value;
}
