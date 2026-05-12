using MediatR;

namespace Application.UseCase.People;

public sealed record DeletePerson(int Id) : IRequest;
