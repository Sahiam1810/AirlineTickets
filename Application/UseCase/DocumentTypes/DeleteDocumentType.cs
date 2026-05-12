using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed record DeleteDocumentType(int Id) : IRequest;
