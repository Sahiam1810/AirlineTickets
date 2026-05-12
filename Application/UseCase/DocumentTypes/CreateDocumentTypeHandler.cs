using Application.Abstractions;
using Domain.Entities.People;
using MediatR;

namespace Application.UseCase.DocumentTypes;

public sealed class CreateDocumentTypeHandler : IRequestHandler<CreateDocumentType, int>
{
    private readonly IUnitOfWork uow;

    public CreateDocumentTypeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateDocumentType req, CancellationToken ct)
    {
        if (await uow.DocumentTypes.ExistsByCodeAsync(req.Code, ct))
            throw new Exception($"DocumentType with code {req.Code} already exists.");

        if (await uow.DocumentTypes.ExistsByNameAsync(req.Name, ct))
            throw new Exception($"DocumentType with name {req.Name} already exists.");

        var documentType = new DocumentType(req.Name, req.Code);
        await uow.DocumentTypes.AddAsync(documentType, ct);
        await uow.SaveChangesAsync(ct);
        return documentType.Id;
    }
}
