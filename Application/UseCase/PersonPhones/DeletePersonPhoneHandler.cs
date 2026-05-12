using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PersonPhones;

public sealed class DeletePersonPhoneHandler(IUnitOfWork uow) : IRequestHandler<DeletePersonPhone>
{
    public async Task Handle(DeletePersonPhone request, CancellationToken ct)
    {
        var personPhone = await uow.PersonPhones.GetByIdAsync(request.Id, ct);

        if (personPhone is null)
            throw new Exception($"PersonPhone with id {request.Id} not found.");

        await uow.PersonPhones.RemoveAsync(personPhone, ct);
        await uow.SaveChangesAsync(ct);
    }
}
