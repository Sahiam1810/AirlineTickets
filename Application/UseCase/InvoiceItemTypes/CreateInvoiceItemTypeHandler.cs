using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class CreateInvoiceItemTypeHandler : IRequestHandler<CreateInvoiceItemType, int>
{
    private readonly IUnitOfWork _uow;

    public CreateInvoiceItemTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateInvoiceItemType req, CancellationToken ct)
    {
        if (await _uow.InvoiceItemTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Invoice item type with name '{req.Name}' already exists.");
        }

        var invoiceItemType = new InvoiceItemType(req.Name);
        
        await _uow.InvoiceItemTypes.AddAsync(invoiceItemType, ct);
        await _uow.SaveChangesAsync(ct);

        return invoiceItemType.Id;
    }
}
