using System;
using Domain.Common;

namespace Domain.Entities.Payments;

public sealed class PaymentState : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    // Navigation
    public ICollection<Payment> Payments { get; set; } = [];

    private PaymentState() { }

    public PaymentState(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}