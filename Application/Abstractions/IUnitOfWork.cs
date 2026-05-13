using System;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    IContinent Continents { get; }
    ICountryRepository Countries { get; }
    IRegionRepository Regions { get; }
    ICityRepository Cities { get; }
    IRoadTypeRepository RoadTypes { get; }
    IAddressRepository Addresses { get; }
    IDocumentTypeRepository DocumentTypes { get; }
    IPersonRepository People { get; }
    IEmailDomainRepository EmailDomains { get; }
    IPhoneCodeRepository PhoneCodes { get; }
    IPersonEmailRepository PersonEmails { get; }
    IPersonPhoneRepository PersonPhones { get; }
    IClientRepository Clients { get; }
    IPassengerTypeRepository PassengerTypes { get; }
    IPassengerRepository Passengers { get; }
    IAirlineRepository Airlines { get; }
    IAirportRepository Airports { get; }
    IAirportAirlineRepository AirportAirlines { get; }
    IAircraftManufacturerRepository AircraftManufacturers { get; }
    IAircraftModelRepository AircraftModels { get; }
    IAircraftRepository Aircraft { get; }
    ICabinTypeRepository CabinTypes { get; }
    ICabinConfigurationRepository CabinConfigurations { get; }
    IStaffRoleRepository StaffRoles { get; }
    IAvailabilityStatusRepository AvailabilityStatuses { get; }
    IStaffAvailabilityRepository StaffAvailabilities { get; }
    IStaffRepository Staff { get; }
    IRouteRepository Routes { get; }
    IRouteStopRepository RouteStops { get; }
    ISeasonRepository Seasons { get; }
    IFareRepository Fares { get; }
    IFlightStateRepository FlightStates { get; }
    IFlightStatusTransitionRepository FlightStatusTransitions { get; }
    IFlightRoleRepository FlightRoles { get; }
    IFlightRepository Flights { get; }
    IFlightAssignmentRepository FlightAssignments { get; }
    ISeatLocationTypeRepository SeatLocationTypes { get; }
    IFlightSeatRepository FlightSeats { get; }
    IReservationStatusRepository ReservationStatuses { get; }
    IReservationStatusTransitionRepository ReservationStatusTransitions { get; }
    IReservationRepository Reservations { get; }
    IReservationFlightRepository ReservationFlights { get; }
    IReservationPassengerRepository ReservationPassengers { get; }
    ITicketStatusRepository TicketStatuses { get; }
    ITicketRepository Tickets { get; }
    ICheckInStatusRepository CheckInStatuses { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
}
