using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.EmailDomains;
using MediatR;

namespace Application.UseCase.EmailDomains;

public sealed class CreateEmailDomainHandler : IRequestHandler<CreateEmailDomain, int>
{
    private readonly IUnitOfWork uow;

    public CreateEmailDomainHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateEmailDomain req, CancellationToken ct)
    {
        if (await uow.EmailDomains.ExistsAsync(req.Domain, ct))
            throw new Exception($"EmailDomain with domain {req.Domain} already exists.");

        var domain = EmailDomainValue.Create(req.Domain);
        var emailDomain = new EmailDomain(domain);

        await uow.EmailDomains.AddAsync(emailDomain, ct);
        await uow.SaveChangesAsync(ct);
        return emailDomain.Id;
    }
}
