namespace Domain.ValueObjects.Auth;

public sealed record Username
{
    public string Value { get; }

    private Username(string value)
    {
        Value = value;
    }

    public static Username Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de usuario es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre de usuario no puede superar los 50 caracteres", nameof(value));

        return new Username(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(Username username) => username.Value;
    public static implicit operator Username(string value) => Create(value);
}
