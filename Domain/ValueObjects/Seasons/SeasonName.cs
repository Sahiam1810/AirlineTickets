namespace Domain.ValueObjects.Seasons;

public sealed record SeasonName
{
    public string Value { get; }

    private SeasonName(string value)
    {
        Value = value;
    }

    public static SeasonName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la temporada es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre de la temporada no puede superar los 50 caracteres", nameof(value));

        if (!normalized.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre de la temporada solo puede contener letras y espacios", nameof(value));

        return new SeasonName(normalized);
    }

    public override string ToString() => Value;
}
