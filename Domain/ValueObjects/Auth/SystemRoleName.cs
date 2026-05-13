namespace Domain.ValueObjects.Auth;

public sealed record SystemRoleName
{
    public string Value { get; }

    private SystemRoleName(string value)
    {
        Value = value;
    }

    public static SystemRoleName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del rol es obligatorio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new ArgumentException("El nombre del rol no puede superar los 50 caracteres", nameof(value));

        return new SystemRoleName(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(SystemRoleName name) => name.Value;
    public static implicit operator SystemRoleName(string value) => Create(value);
}
