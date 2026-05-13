using System;
using MediatR;

namespace Application.UseCase.Reservations;

public sealed record CreateReservation(
    string ReservationCode, 
    int ClientId, 
    int ReservationStatusId, 
    decimal TotalValue, 
    DateTime? ExpiresAt) : IRequest<int>;
