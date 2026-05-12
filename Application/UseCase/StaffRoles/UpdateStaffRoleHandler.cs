using Application.Abstractions;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed class UpdateStaffRoleHandler(IUnitOfWork uow) : IRequestHandler<UpdateStaffRole>
{
    public async Task Handle(UpdateStaffRole request, CancellationToken ct)
    {
        var staffRole = await uow.StaffRoles.GetByIdAsync(request.Id, ct);

        if (staffRole is null)
            throw new Exception($"StaffRole with id {request.Id} not found.");

        var name = StaffRoleName.Create(request.Name);
        var sameName = string.Equals(staffRole.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.StaffRoles.ExistsByNameAsync(name.Value, ct))
            throw new Exception($"StaffRole with name {name.Value} already exists.");

        staffRole.Update(name);

        await uow.StaffRoles.UpdateAsync(staffRole, ct);
        await uow.SaveChangesAsync(ct);
    }
}
