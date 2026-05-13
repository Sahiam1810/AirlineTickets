using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed record DeleteSystemRole(int Id) : IRequest;
