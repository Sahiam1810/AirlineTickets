using System.Net;

namespace Domain.ValueObjects.Auth;

public sealed record IpAddress
{
    public string Value { get; }

    private IpAddress(string value)
    {
        Value = value;
    }

    public static IpAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La dirección IP es obligatoria", nameof(value));

        var normalized = value.Trim();

        if (!IPAddress.TryParse(normalized, out _))
            throw new ArgumentException("La dirección IP no es válida", nameof(value));

        return new IpAddress(normalized);
    }

    public override string ToString() => Value;
    public static implicit operator string(IpAddress ipAddress) => ipAddress.Value;
    public static implicit operator IpAddress(string value) => Create(value);
}
