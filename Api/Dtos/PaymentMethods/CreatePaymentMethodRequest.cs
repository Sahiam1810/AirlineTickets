namespace Api.Dtos.PaymentMethods;

public sealed class CreatePaymentMethodRequest
{
    public int PaymentMethodTypeId { get; init; }
    public int? CardTypeId { get; init; }
    public int? CardIssuerId { get; init; }
    public string CommercialName { get; init; } = string.Empty;
}
