using Application.Abstractions;
using Domain.Entities.Auth;
using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed class UpdateSystemRoleHandler : IRequestHandler<UpdateSystemRole>
{
    private readonly IUnitOfWork _uow;

    public UpdateSystemRoleHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateSystemRole request, CancellationToken ct)
    {
        var role = await _uow.SystemRoles.GetByIdAsync(request.Id, ct);

        if (role is null)
        {
            throw new InvalidOperationException($"Rol con id {request.Id} no encontrado.");
        }

        if (!string.Equals(role.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase) &&
            await _uow.SystemRoles.ExistsByNameAsync(request.Name, ct))
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{request.Name.Trim()}'.");
        }

        role.Update(request.Name, request.Description);
        await _uow.SystemRoles.UpdateAsync(role, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
