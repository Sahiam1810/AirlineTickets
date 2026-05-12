using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.PersonEmails;
using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed class CreatePersonEmailHandler : IRequestHandler<CreatePersonEmail, int>
{
    private readonly IUnitOfWork uow;

    public CreatePersonEmailHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePersonEmail req, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(req.PersonId, ct);
        if (person is null)
            throw new Exception($"Person with id {req.PersonId} not found.");

        var emailDomain = await uow.EmailDomains.GetByIdAsync(req.EmailDomainId, ct);
        if (emailDomain is null)
            throw new Exception($"EmailDomain with id {req.EmailDomainId} not found.");

        var emailUser = EmailUser.Create(req.EmailUser);
        var fullEmail = $"{emailUser.Value}@{emailDomain.Domain.Value}";

        if (await uow.PersonEmails.ExistsAsync(fullEmail, ct))
            throw new Exception($"PersonEmail with email {fullEmail} already exists.");

        if (req.IsPrimary && await uow.PersonEmails.ExistsPrimaryAsync(req.PersonId, ct))
            throw new Exception($"Person with id {req.PersonId} already has a primary email.");

        var personEmail = new PersonEmail(req.PersonId, emailUser, req.EmailDomainId, req.IsPrimary);

        await uow.PersonEmails.AddAsync(personEmail, ct);
        await uow.SaveChangesAsync(ct);
        return personEmail.Id;
    }
}
