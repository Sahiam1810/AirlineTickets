using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed record CreateCabinConfiguration(
    int AircraftId,
    int CabinTypeId,
    int RowStart,
    int RowEnd,
    int SeatsPerRow,
    string SeatLetters) : IRequest<int>;
