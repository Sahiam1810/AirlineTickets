namespace Domain.ValueObjects.Seasons;

public sealed record SeasonDescription
{
    public string Value { get; }

    private SeasonDescription(string value)
    {
        Value = value;
    }

    public static SeasonDescription? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();

        if (normalized.Length > 150)
            throw new ArgumentException("La descripcion de la temporada no puede superar los 150 caracteres", nameof(value));

        return new SeasonDescription(normalized);
    }

    public override string ToString() => Value;
}
