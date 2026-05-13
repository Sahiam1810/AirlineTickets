using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed record DeleteInvoiceItem(int Id) : IRequest;
