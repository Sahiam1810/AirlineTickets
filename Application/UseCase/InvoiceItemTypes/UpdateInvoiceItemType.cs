using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed record UpdateInvoiceItemType(int Id, string Name) : IRequest;
