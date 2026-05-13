using MediatR;

namespace Application.UseCase.Invoices;

public sealed record DeleteInvoice(int Id) : IRequest;
