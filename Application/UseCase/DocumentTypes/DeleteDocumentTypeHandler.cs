using Application.Abstractions;
using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed class DeleteDocumentTypeHandler(IUnitOfWork uow) : IRequestHandler<DeleteDocumentType>
{
    public async Task Handle(DeleteDocumentType request, CancellationToken ct)
    {
        var documentType = await uow.DocumentTypes.GetByIdAsync(request.Id, ct);

        if (documentType is null)
            throw new Exception($"DocumentType with id {request.Id} not found.");

        await uow.DocumentTypes.RemoveAsync(documentType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
