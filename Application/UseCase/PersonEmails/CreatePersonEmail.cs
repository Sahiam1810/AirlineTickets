using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed record CreatePersonEmail(int PersonId, string EmailUser, int EmailDomainId, bool IsPrimary) : IRequest<int>;
