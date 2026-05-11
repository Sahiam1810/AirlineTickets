using System;
using Domain.Common;

namespace Domain.Entities.Payments;

public sealed class PaymentMethod : BaseEntity<int>
{
    public int PaymentMethodTypeId { get; private set; }
    public int? CardTypeId { get; private set; }
    public int? CardIssuerId { get; private set; }
    public string CommercialName { get; private set; } = string.Empty;

    // Navigation
    public PaymentMethodType PaymentMethodType { get; set; } = null!;
    public CardType? CardType { get; set; }
    public CardIssuer? CardIssuer { get; set; }
    public ICollection<Payment> Payments { get; set; } = [];

    private PaymentMethod() { }

    public PaymentMethod(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        PaymentMethodTypeId = paymentMethodTypeId;
        CardTypeId = cardTypeId;
        CardIssuerId = cardIssuerId;
        CommercialName = commercialName;
    }

    public void Update(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        PaymentMethodTypeId = paymentMethodTypeId;
        CardTypeId = cardTypeId;
        CardIssuerId = cardIssuerId;
        CommercialName = commercialName;
    }
}