using Application.Abstractions;
using Domain.ValueObjects.EmailDomains;
using MediatR;

namespace Application.UseCase.EmailDomains;

public sealed class UpdateEmailDomainHandler(IUnitOfWork uow) : IRequestHandler<UpdateEmailDomain>
{
    public async Task Handle(UpdateEmailDomain request, CancellationToken ct)
    {
        var emailDomain = await uow.EmailDomains.GetByIdAsync(request.Id, ct);

        if (emailDomain is null)
            throw new Exception($"EmailDomain with id {request.Id} not found.");

        var normalized = EmailDomainValue.Create(request.Domain);
        var sameDomain = string.Equals(emailDomain.Domain.Value, normalized.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameDomain && await uow.EmailDomains.ExistsAsync(normalized.Value, ct))
            throw new Exception($"EmailDomain with domain {request.Domain} already exists.");

        emailDomain.Update(normalized);

        await uow.EmailDomains.UpdateAsync(emailDomain, ct);
        await uow.SaveChangesAsync(ct);
    }
}
