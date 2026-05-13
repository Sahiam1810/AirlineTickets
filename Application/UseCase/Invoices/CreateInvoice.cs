using MediatR;

namespace Application.UseCase.Invoices;

public sealed record CreateInvoice(
    int ReservationId,
    string InvoiceNumber,
    DateTime IssuedAt,
    decimal Subtotal,
    decimal Taxes,
    decimal Total) : IRequest<int>;
