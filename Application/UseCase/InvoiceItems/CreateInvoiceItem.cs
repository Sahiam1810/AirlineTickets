using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed record CreateInvoiceItem(
    int InvoiceId,
    int InvoiceItemTypeId,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    int? ReservationPassengerId) : IRequest<int>;
