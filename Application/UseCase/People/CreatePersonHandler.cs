using Application.Abstractions;
using Domain.Entities.People;
using MediatR;

namespace Application.UseCase.People;

public sealed class CreatePersonHandler : IRequestHandler<CreatePerson, int>
{
    private readonly IUnitOfWork uow;

    public CreatePersonHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePerson req, CancellationToken ct)
    {
        var documentType = await uow.DocumentTypes.GetByIdAsync(req.DocumentTypeId, ct);
        if (documentType is null)
            throw new Exception($"DocumentType with id {req.DocumentTypeId} not found.");

        if (req.AddressId.HasValue)
        {
            var address = await uow.Addresses.GetByIdAsync(req.AddressId.Value, ct);
            if (address is null)
                throw new Exception($"Address with id {req.AddressId.Value} not found.");
        }

        if (await uow.People.ExistsAsync(req.DocumentTypeId, req.DocumentNumber, ct))
            throw new Exception($"Person with document {req.DocumentNumber} already exists for document type {req.DocumentTypeId}.");

        var person = new Person(
            req.DocumentTypeId,
            req.DocumentNumber,
            req.FirstName,
            req.LastName,
            req.BirthDate,
            req.Gender,
            req.AddressId);

        await uow.People.AddAsync(person, ct);
        await uow.SaveChangesAsync(ct);
        return person.Id;
    }
}
