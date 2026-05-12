using Application.Abstractions;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed class CreateStaffRoleHandler : IRequestHandler<CreateStaffRole, int>
{
    private readonly IUnitOfWork uow;

    public CreateStaffRoleHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateStaffRole req, CancellationToken ct)
    {
        var name = StaffRoleName.Create(req.Name);

        if (await uow.StaffRoles.ExistsByNameAsync(name.Value, ct))
            throw new Exception($"StaffRole with name {name.Value} already exists.");

        var staffRole = new StaffRole(name);

        await uow.StaffRoles.AddAsync(staffRole, ct);
        await uow.SaveChangesAsync(ct);
        return staffRole.Id;
    }
}
