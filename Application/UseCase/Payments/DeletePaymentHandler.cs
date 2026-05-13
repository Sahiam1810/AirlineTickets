using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Payments;

public sealed class DeletePaymentHandler : IRequestHandler<DeletePayment>
{
    private readonly IUnitOfWork _uow;

    public DeletePaymentHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeletePayment req, CancellationToken ct)
    {
        var payment = await _uow.Payments.GetByIdAsync(req.Id, ct);
        if (payment is null)
        {
            throw new KeyNotFoundException($"Payment with id {req.Id} was not found.");
        }

        await _uow.Payments.RemoveAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
