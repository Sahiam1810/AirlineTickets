using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed record UpdateCabinConfiguration(
    int Id,
    int AircraftId,
    int CabinTypeId,
    int RowStart,
    int RowEnd,
    int SeatsPerRow,
    string SeatLetters) : IRequest;
