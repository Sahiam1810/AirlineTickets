using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Invoices;

public sealed class UpdateInvoiceHandler : IRequestHandler<UpdateInvoice>
{
    private readonly IUnitOfWork _uow;

    public UpdateInvoiceHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateInvoice req, CancellationToken ct)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(req.Id, ct);
        if (invoice is null)
        {
            throw new KeyNotFoundException($"Invoice with id {req.Id} was not found.");
        }

        if (await _uow.Reservations.GetByIdAsync(req.ReservationId, ct) is null)
        {
            throw new KeyNotFoundException($"Reservation with id {req.ReservationId} was not found.");
        }

        if (invoice.ReservationId != req.ReservationId &&
            await _uow.Invoices.ExistsByReservationAsync(req.ReservationId, ct))
        {
            throw new InvalidOperationException($"Reservation with id {req.ReservationId} already has an invoice.");
        }

        var normalizedInvoiceNumber = req.InvoiceNumber?.Trim() ?? string.Empty;

        if (!string.Equals(invoice.InvoiceNumber, normalizedInvoiceNumber, StringComparison.Ordinal) &&
            await _uow.Invoices.ExistsAsync(normalizedInvoiceNumber, ct))
        {
            throw new InvalidOperationException($"Invoice with number '{req.InvoiceNumber}' already exists.");
        }

        invoice.Update(
            req.ReservationId,
            normalizedInvoiceNumber,
            req.IssuedAt,
            req.Subtotal,
            req.Taxes,
            req.Total);

        await _uow.Invoices.UpdateAsync(invoice, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
