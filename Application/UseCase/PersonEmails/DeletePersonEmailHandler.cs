using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed class DeletePersonEmailHandler(IUnitOfWork uow) : IRequestHandler<DeletePersonEmail>
{
    public async Task Handle(DeletePersonEmail request, CancellationToken ct)
    {
        var personEmail = await uow.PersonEmails.GetByIdAsync(request.Id, ct);

        if (personEmail is null)
            throw new Exception($"PersonEmail with id {request.Id} not found.");

        await uow.PersonEmails.RemoveAsync(personEmail, ct);
        await uow.SaveChangesAsync(ct);
    }
}
