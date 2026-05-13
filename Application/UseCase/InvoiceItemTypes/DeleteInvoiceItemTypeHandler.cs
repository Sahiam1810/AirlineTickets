using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class DeleteInvoiceItemTypeHandler : IRequestHandler<DeleteInvoiceItemType>
{
    private readonly IUnitOfWork _uow;

    public DeleteInvoiceItemTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteInvoiceItemType req, CancellationToken ct)
    {
        var invoiceItemType = await _uow.InvoiceItemTypes.GetByIdAsync(req.Id, ct);
        if (invoiceItemType is null)
        {
            throw new KeyNotFoundException($"Invoice item type with id {req.Id} was not found.");
        }

        await _uow.InvoiceItemTypes.RemoveAsync(invoiceItemType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
