using MediatR;

namespace Application.UseCase.Permissions;

public sealed record DeletePermission(int Id) : IRequest;
