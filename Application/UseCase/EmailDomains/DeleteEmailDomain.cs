using MediatR;

namespace Application.UseCase.EmailDomains;

public sealed record DeleteEmailDomain(int Id) : IRequest;
