namespace Domain.ValueObjects.Auth;

public sealed record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static PasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El hash de contraseña es obligatorio", nameof(value));

        var normalized = value.Trim();

        return new PasswordHash(normalized);
    }

    public override string ToString() => Value;
    public static implicit operator string(PasswordHash hash) => hash.Value;
    public static implicit operator PasswordHash(string value) => Create(value);
}
