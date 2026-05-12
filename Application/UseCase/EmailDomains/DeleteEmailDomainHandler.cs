using Application.Abstractions;
using MediatR;

namespace Application.UseCase.EmailDomains;

public sealed class DeleteEmailDomainHandler(IUnitOfWork uow) : IRequestHandler<DeleteEmailDomain>
{
    public async Task Handle(DeleteEmailDomain request, CancellationToken ct)
    {
        var emailDomain = await uow.EmailDomains.GetByIdAsync(request.Id, ct);

        if (emailDomain is null)
            throw new Exception($"EmailDomain with id {request.Id} not found.");

        await uow.EmailDomains.RemoveAsync(emailDomain, ct);
        await uow.SaveChangesAsync(ct);
    }
}
