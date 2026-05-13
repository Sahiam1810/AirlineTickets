using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed record UpdateSystemRole(int Id, string Name, string? Description) : IRequest;
