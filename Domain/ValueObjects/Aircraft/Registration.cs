using System.Text.RegularExpressions;

namespace Domain.ValueObjects.Aircraft;

public sealed record Registration
{
    private static readonly Regex ValidPattern = new("^[A-Z0-9-]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Registration(string value)
    {
        Value = value;
    }

    public static Registration Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Registration is required", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 20)
            throw new ArgumentException("Registration cannot exceed 20 characters", nameof(value));

        if (!ValidPattern.IsMatch(normalized))
            throw new ArgumentException("Registration can only contain letters, numbers and hyphens", nameof(value));

        return new Registration(normalized);
    }

    public override string ToString() => Value;
}
