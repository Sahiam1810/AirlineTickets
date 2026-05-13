using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed class DeleteInvoiceItemHandler : IRequestHandler<DeleteInvoiceItem>
{
    private readonly IUnitOfWork _uow;

    public DeleteInvoiceItemHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteInvoiceItem req, CancellationToken ct)
    {
        var invoiceItem = await _uow.InvoiceItems.GetByIdAsync(req.Id, ct);
        if (invoiceItem is null)
            throw new KeyNotFoundException($"Invoice item with id {req.Id} was not found.");

        await _uow.InvoiceItems.RemoveAsync(invoiceItem, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
