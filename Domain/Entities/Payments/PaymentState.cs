using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities.Payments;

public sealed class PaymentState : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    // Navigation
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    private PaymentState() { }

    public PaymentState(string name)
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
            throw new ArgumentException("Name cannot be null or empty.");

        if (name.Length > 50)
            throw new ArgumentException("Name cannot exceed 50 characters.");

        Name = name.Trim();
    }
}