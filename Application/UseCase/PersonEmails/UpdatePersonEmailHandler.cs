using Application.Abstractions;
using Domain.ValueObjects.PersonEmails;
using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed class UpdatePersonEmailHandler(IUnitOfWork uow) : IRequestHandler<UpdatePersonEmail>
{
    public async Task Handle(UpdatePersonEmail request, CancellationToken ct)
    {
        var personEmail = await uow.PersonEmails.GetByIdAsync(request.Id, ct);

        if (personEmail is null)
            throw new Exception($"PersonEmail with id {request.Id} not found.");

        var emailDomain = await uow.EmailDomains.GetByIdAsync(request.EmailDomainId, ct);
        if (emailDomain is null)
            throw new Exception($"EmailDomain with id {request.EmailDomainId} not found.");

        var emailUser = EmailUser.Create(request.EmailUser);
        var fullEmail = $"{emailUser.Value}@{emailDomain.Domain.Value}";
        var currentEmail = $"{personEmail.EmailUser.Value}@{personEmail.EmailDomain.Domain.Value}";
        var sameEmail = string.Equals(currentEmail, fullEmail, StringComparison.OrdinalIgnoreCase);

        if (!sameEmail && await uow.PersonEmails.ExistsAsync(fullEmail, ct))
            throw new Exception($"PersonEmail with email {fullEmail} already exists.");

        if (request.IsPrimary && !personEmail.IsPrimary && await uow.PersonEmails.ExistsPrimaryAsync(personEmail.PersonId, ct))
            throw new Exception($"Person with id {personEmail.PersonId} already has a primary email.");

        personEmail.Update(emailUser, request.EmailDomainId, request.IsPrimary);

        await uow.PersonEmails.UpdateAsync(personEmail, ct);
        await uow.SaveChangesAsync(ct);
    }
}
