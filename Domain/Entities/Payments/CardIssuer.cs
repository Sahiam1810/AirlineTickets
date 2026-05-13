using System;
using Domain.Common;

namespace Domain.Entities.Payments;

public sealed class CardIssuer : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    public ICollection<PaymentMethod> PaymentMethods { get; set; } = [];

    private CardIssuer() { }

    public CardIssuer(string name)
    {
        ValidateAndSetName(name);
    }

    public void Update(string name)
    {
        ValidateAndSetName(name);
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }

        if (name.Length > 50)
        {
            throw new ArgumentException("Name cannot exceed 50 characters.");
        }

        Name = name.Trim();
    }
}
