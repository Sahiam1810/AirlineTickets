using Application.Abstractions;
using Domain.Entities.Auth;
using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed class CreateSystemRoleHandler : IRequestHandler<CreateSystemRole, int>
{
    private readonly IUnitOfWork _uow;

    public CreateSystemRoleHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateSystemRole request, CancellationToken ct)
    {
        if (await _uow.SystemRoles.ExistsByNameAsync(request.Name, ct))
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{request.Name.Trim()}'.");
        }

        var role = new SystemRole(request.Name, request.Description);
        await _uow.SystemRoles.AddAsync(role, ct);
        await _uow.SaveChangesAsync(ct);
        return role.Id;
    }
}
