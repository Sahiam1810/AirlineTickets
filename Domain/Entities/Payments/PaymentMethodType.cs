using System;
using Domain.Common;

namespace Domain.Entities.Payments;

public sealed class PaymentMethodType : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    // Navigation
    public ICollection<PaymentMethod> PaymentMethods { get; set; } = [];

    private PaymentMethodType() { }

    public PaymentMethodType(string name)

    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}