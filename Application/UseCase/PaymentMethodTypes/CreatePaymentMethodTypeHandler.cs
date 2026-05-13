using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class CreatePaymentMethodTypeHandler : IRequestHandler<CreatePaymentMethodType, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePaymentMethodTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreatePaymentMethodType req, CancellationToken ct)
    {
        if (await _uow.PaymentMethodTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Payment method type with name '{req.Name}' already exists.");
        }

        var paymentMethodType = new PaymentMethodType(req.Name);

        await _uow.PaymentMethodTypes.AddAsync(paymentMethodType, ct);
        await _uow.SaveChangesAsync(ct);

        return paymentMethodType.Id;
    }
}
