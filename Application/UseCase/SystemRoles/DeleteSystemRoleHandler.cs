using Application.Abstractions;
using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed class DeleteSystemRoleHandler : IRequestHandler<DeleteSystemRole>
{
    private readonly IUnitOfWork _uow;

    public DeleteSystemRoleHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteSystemRole request, CancellationToken ct)
    {
        var role = await _uow.SystemRoles.GetByIdAsync(request.Id, ct);

        if (role is null)
        {
            throw new InvalidOperationException($"Rol con id {request.Id} no encontrado.");
        }

        await _uow.SystemRoles.RemoveAsync(role, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
