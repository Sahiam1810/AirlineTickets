using MediatR;

namespace Application.UseCase.Permissions;

public sealed record UpdatePermission(int Id, string Name, string? Description) : IRequest;
