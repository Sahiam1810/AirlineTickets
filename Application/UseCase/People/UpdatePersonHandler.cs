using Application.Abstractions;
using MediatR;

namespace Application.UseCase.People;

public sealed class UpdatePersonHandler(IUnitOfWork uow) : IRequestHandler<UpdatePerson>
{
    public async Task Handle(UpdatePerson request, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(request.Id, ct);

        if (person is null)
            throw new Exception($"Person with id {request.Id} not found.");

        var documentType = await uow.DocumentTypes.GetByIdAsync(request.DocumentTypeId, ct);
        if (documentType is null)
            throw new Exception($"DocumentType with id {request.DocumentTypeId} not found.");

        if (request.AddressId.HasValue)
        {
            var address = await uow.Addresses.GetByIdAsync(request.AddressId.Value, ct);
            if (address is null)
                throw new Exception($"Address with id {request.AddressId.Value} not found.");
        }

        var sameDocumentType = person.DocumentTypeId == request.DocumentTypeId;
        var sameDocumentNumber = string.Equals(person.DocumentNumber.Value, request.DocumentNumber.Trim(), StringComparison.OrdinalIgnoreCase);

        if ((!sameDocumentType || !sameDocumentNumber) && await uow.People.ExistsAsync(request.DocumentTypeId, request.DocumentNumber, ct))
            throw new Exception($"Person with document {request.DocumentNumber} already exists for document type {request.DocumentTypeId}.");

        person.Update(
            request.DocumentTypeId,
            request.DocumentNumber,
            request.FirstName,
            request.LastName,
            request.BirthDate,
            request.Gender,
            request.AddressId
        );

        await uow.People.UpdateAsync(person, ct);
        await uow.SaveChangesAsync(ct);
    }
}
