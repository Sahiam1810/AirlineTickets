using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Invoices;

public sealed class DeleteInvoiceHandler : IRequestHandler<DeleteInvoice>
{
    private readonly IUnitOfWork _uow;

    public DeleteInvoiceHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteInvoice req, CancellationToken ct)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(req.Id, ct);
        if (invoice is null)
        {
            throw new KeyNotFoundException($"Invoice with id {req.Id} was not found.");
        }

        await _uow.Invoices.RemoveAsync(invoice, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
