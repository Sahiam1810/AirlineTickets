namespace Domain.ValueObjects.Auth;

public sealed record PermissionName
{
    public string Value { get; }

    private PermissionName(string value)
    {
        Value = value;
    }

    public static PermissionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del permiso es obligatorio", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 100)
            throw new ArgumentException("El nombre del permiso no puede superar los 100 caracteres", nameof(value));

        return new PermissionName(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(PermissionName name) => name.Value;
    public static implicit operator PermissionName(string value) => Create(value);
}
