using MediatR;

namespace Application.UseCase.PersonEmails;

public sealed record DeletePersonEmail(int Id) : IRequest;
