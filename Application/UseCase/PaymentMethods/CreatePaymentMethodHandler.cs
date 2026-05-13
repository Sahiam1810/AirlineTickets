using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed class CreatePaymentMethodHandler : IRequestHandler<CreatePaymentMethod, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePaymentMethodHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreatePaymentMethod req, CancellationToken ct)
    {
        await ValidateReferencesAsync(req.PaymentMethodTypeId, req.CardTypeId, req.CardIssuerId, ct);

        if (await _uow.PaymentMethods.ExistsAsync(req.CommercialName, ct))
        {
            throw new InvalidOperationException($"Payment method with commercial name '{req.CommercialName}' already exists.");
        }

        var paymentMethod = new PaymentMethod(
            req.PaymentMethodTypeId,
            req.CardTypeId,
            req.CardIssuerId,
            req.CommercialName);

        await _uow.PaymentMethods.AddAsync(paymentMethod, ct);
        await _uow.SaveChangesAsync(ct);

        return paymentMethod.Id;
    }

    private async Task ValidateReferencesAsync(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, CancellationToken ct)
    {
        if (cardTypeId.HasValue != cardIssuerId.HasValue)
        {
            throw new InvalidOperationException("Card type and card issuer must be provided together.");
        }

        if (await _uow.PaymentMethodTypes.GetByIdAsync(paymentMethodTypeId, ct) is null)
        {
            throw new InvalidOperationException($"Payment method type with id {paymentMethodTypeId} was not found.");
        }

        if (cardTypeId.HasValue && await _uow.CardTypes.GetByIdAsync(cardTypeId.Value, ct) is null)
        {
            throw new InvalidOperationException($"Card type with id {cardTypeId.Value} was not found.");
        }

        if (cardIssuerId.HasValue && await _uow.CardIssuers.GetByIdAsync(cardIssuerId.Value, ct) is null)
        {
            throw new InvalidOperationException($"Card issuer with id {cardIssuerId.Value} was not found.");
        }
    }
}
