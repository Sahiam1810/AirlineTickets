using Application.Abstractions;
using Domain.Entities.Reservations;
using MediatR;

namespace Application.UseCase.ReservationStatusTransitions;

public sealed class CreateReservationStatusTransitionHandler : IRequestHandler<CreateReservationStatusTransition, int>
{
    private readonly IUnitOfWork uow;

    public CreateReservationStatusTransitionHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateReservationStatusTransition req, CancellationToken ct)
    {
        ValidateDifferentStatuses(req.FromStatusId, req.ToStatusId);
        await ValidateReferences(req.FromStatusId, req.ToStatusId, ct);

        if (await uow.ReservationStatusTransitions.ExistsAsync(req.FromStatusId, req.ToStatusId, null, ct))
            throw new Exception($"ReservationStatusTransition from status id {req.FromStatusId} to status id {req.ToStatusId} already exists.");

        var reservationStatusTransition = new ReservationStatusTransition(req.FromStatusId, req.ToStatusId);

        await uow.ReservationStatusTransitions.AddAsync(reservationStatusTransition, ct);
        await uow.SaveChangesAsync(ct);
        return reservationStatusTransition.Id;
    }

    private static void ValidateDifferentStatuses(int fromStatusId, int toStatusId)
    {
        if (fromStatusId == toStatusId)
            throw new Exception("FromStatusId and ToStatusId must be different.");
    }

    private async Task ValidateReferences(int fromStatusId, int toStatusId, CancellationToken ct)
    {
        if (await uow.ReservationStatuses.GetByIdAsync(fromStatusId, ct) is null)
            throw new Exception($"ReservationStatus with id {fromStatusId} not found.");

        if (await uow.ReservationStatuses.GetByIdAsync(toStatusId, ct) is null)
            throw new Exception($"ReservationStatus with id {toStatusId} not found.");
    }
}
