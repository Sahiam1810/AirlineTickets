using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed class DeletePaymentMethodHandler : IRequestHandler<DeletePaymentMethod>
{
    private readonly IUnitOfWork _uow;

    public DeletePaymentMethodHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeletePaymentMethod req, CancellationToken ct)
    {
        var paymentMethod = await _uow.PaymentMethods.GetByIdAsync(req.Id, ct);
        if (paymentMethod is null)
        {
            throw new KeyNotFoundException($"Payment method with id {req.Id} was not found.");
        }

        await _uow.PaymentMethods.RemoveAsync(paymentMethod, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
