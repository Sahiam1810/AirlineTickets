using Application.Abstractions;
using Domain.Entities.Auth;
using MediatR;

namespace Application.UseCase.Permissions;

public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermission>
{
    private readonly IUnitOfWork _uow;

    public UpdatePermissionHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdatePermission request, CancellationToken ct)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.Id, ct);

        if (permission is null)
        {
            throw new InvalidOperationException($"Permiso con id {request.Id} no encontrado.");
        }

        if (!string.Equals(permission.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase) &&
            await _uow.Permissions.ExistsByNameAsync(request.Name, ct))
        {
            throw new InvalidOperationException($"Ya existe un permiso con el nombre '{request.Name.Trim()}'.");
        }

        permission.Update(request.Name, request.Description);
        await _uow.Permissions.UpdateAsync(permission, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
