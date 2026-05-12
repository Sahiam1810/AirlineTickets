using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed record CreateDocumentType(string Name, string Code) : IRequest<int>;
