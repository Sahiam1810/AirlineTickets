using System;
using MediatR;

namespace Application.UseCase.Reservations;

public sealed record UpdateReservation(
    int Id,
    int ReservationStatusId, 
    decimal TotalValue, 
    DateTime? ExpiresAt) : IRequest;
