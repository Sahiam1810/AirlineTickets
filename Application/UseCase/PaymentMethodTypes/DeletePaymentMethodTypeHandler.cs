using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class DeletePaymentMethodTypeHandler : IRequestHandler<DeletePaymentMethodType>
{
    private readonly IUnitOfWork _uow;

    public DeletePaymentMethodTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeletePaymentMethodType req, CancellationToken ct)
    {
        var paymentMethodType = await _uow.PaymentMethodTypes.GetByIdAsync(req.Id, ct);
        if (paymentMethodType is null)
        {
            throw new KeyNotFoundException($"Payment method type with id {req.Id} was not found.");
        }

        await _uow.PaymentMethodTypes.RemoveAsync(paymentMethodType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
