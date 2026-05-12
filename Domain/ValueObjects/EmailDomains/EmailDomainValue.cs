using System.Text.RegularExpressions;

namespace Domain.ValueObjects.EmailDomains;

public sealed partial record EmailDomainValue
{
    public string Value { get; }

    private EmailDomainValue(string value)
    {
        Value = value;
    }

    public static EmailDomainValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El dominio de correo es obligatorio", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length > 100)
            throw new ArgumentException("El dominio no puede superar los 100 caracteres", nameof(value));

        if (normalized.Any(char.IsWhiteSpace))
            throw new ArgumentException("El dominio no puede contener espacios", nameof(value));

        if (!DomainRegex().IsMatch(normalized))
            throw new ArgumentException("El dominio debe tener un formato valido", nameof(value));

        return new EmailDomainValue(normalized);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^(?!-)(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex DomainRegex();
}
