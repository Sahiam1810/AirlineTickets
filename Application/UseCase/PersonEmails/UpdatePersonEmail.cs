using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed record UpdatePersonEmail(int Id, string EmailUser, int EmailDomainId, bool IsPrimary) : IRequest;
