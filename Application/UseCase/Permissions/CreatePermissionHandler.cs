using Application.Abstractions;
using Domain.Entities.Auth;
using MediatR;

namespace Application.UseCase.Permissions;

public sealed class CreatePermissionHandler : IRequestHandler<CreatePermission, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePermissionHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreatePermission request, CancellationToken ct)
    {
        if (await _uow.Permissions.ExistsByNameAsync(request.Name, ct))
        {
            throw new InvalidOperationException($"Ya existe un permiso con el nombre '{request.Name.Trim()}'.");
        }

        var permission = new Permission(request.Name, request.Description);
        await _uow.Permissions.AddAsync(permission, ct);
        await _uow.SaveChangesAsync(ct);
        return permission.Id;
    }
}
