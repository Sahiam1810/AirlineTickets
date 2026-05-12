using MediatR;

namespace Application.UseCase.PersonPhones;

public sealed record DeletePersonPhone(int Id) : IRequest;
