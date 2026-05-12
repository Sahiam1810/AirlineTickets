using Application.Abstractions;
using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed class UpdateDocumentTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdateDocumentType>
{
    public async Task Handle(UpdateDocumentType request, CancellationToken ct)
    {
        var documentType = await uow.DocumentTypes.GetByIdAsync(request.Id, ct);

        if (documentType is null)
            throw new Exception($"DocumentType with id {request.Id} not found.");

        var sameCode = string.Equals(documentType.Code.Value, request.Code.Trim(), StringComparison.OrdinalIgnoreCase);
        var sameName = string.Equals(documentType.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);

        if (!sameCode && await uow.DocumentTypes.ExistsByCodeAsync(request.Code, ct))
            throw new Exception($"DocumentType with code {request.Code} already exists.");

        if (!sameName && await uow.DocumentTypes.ExistsByNameAsync(request.Name, ct))
            throw new Exception($"DocumentType with name {request.Name} already exists.");

        documentType.Update(
            request.Name,
            request.Code
        );

        await uow.DocumentTypes.UpdateAsync(documentType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
