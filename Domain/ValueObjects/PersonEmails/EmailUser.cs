namespace Domain.ValueObjects.PersonEmails;

public sealed record EmailUser
{
    public string Value { get; }

    private EmailUser(string value)
    {
        Value = value;
    }

    public static EmailUser Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email user is required", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length > 100)
            throw new ArgumentException("Email user cannot exceed 100 characters", nameof(value));

        if (!normalized.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-'))
            throw new ArgumentException("Email user contains invalid characters", nameof(value));

        return new EmailUser(normalized);
    }

    public override string ToString() => Value;
}
