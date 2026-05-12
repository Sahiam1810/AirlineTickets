using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed record UpdateDocumentType(int Id, string Name, string Code) : IRequest;
