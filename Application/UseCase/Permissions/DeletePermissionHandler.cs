using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Permissions;

public sealed class DeletePermissionHandler : IRequestHandler<DeletePermission>
{
    private readonly IUnitOfWork _uow;

    public DeletePermissionHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeletePermission request, CancellationToken ct)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.Id, ct);

        if (permission is null)
        {
            throw new InvalidOperationException($"Permiso con id {request.Id} no encontrado.");
        }

        await _uow.Permissions.RemoveAsync(permission, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
