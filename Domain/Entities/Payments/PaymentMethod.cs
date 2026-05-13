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
        Validate(paymentMethodTypeId, cardTypeId, cardIssuerId, commercialName);

        PaymentMethodTypeId = paymentMethodTypeId;
        CardTypeId = cardTypeId;
        CardIssuerId = cardIssuerId;
        CommercialName = commercialName.Trim();
    }

    public void Update(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        Validate(paymentMethodTypeId, cardTypeId, cardIssuerId, commercialName);

        PaymentMethodTypeId = paymentMethodTypeId;
        CardTypeId = cardTypeId;
        CardIssuerId = cardIssuerId;
        CommercialName = commercialName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        if (paymentMethodTypeId <= 0)
        {
            throw new ArgumentException("Payment method type id must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(commercialName))
        {
            throw new ArgumentException("Commercial name cannot be null or empty.");
        }

        if (commercialName.Trim().Length > 50)
        {
            throw new ArgumentException("Commercial name cannot exceed 50 characters.");
        }

        if (cardTypeId.HasValue && cardTypeId.Value <= 0)
        {
            throw new ArgumentException("Card type id must be greater than zero.");
        }

        if (cardIssuerId.HasValue && cardIssuerId.Value <= 0)
        {
            throw new ArgumentException("Card issuer id must be greater than zero.");
        }

        if (cardTypeId.HasValue != cardIssuerId.HasValue)
        {
            throw new ArgumentException("Card type and card issuer must be provided together.");
        }
    }
}
