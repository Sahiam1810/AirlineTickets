using Domain.Entities.Aircraft;
using Domain.Entities.Airlines;
using Domain.Entities.Auth;
using Domain.Entities.Flights;
using Domain.Entities.Geography;
using Domain.Entities.Location;
using Domain.Entities.Payments;
using Domain.Entities.People;
using Domain.Entities.Reservations;
using Domain.Entities.Routes;
using Domain.Entities.Staff;
using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Geography
    public DbSet<Continent> Continents => Set<Continent>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<City> Cities => Set<City>();

    // Location
    public DbSet<RoadType> RoadTypes => Set<RoadType>();
    public DbSet<Address> Addresses => Set<Address>();

    // People
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<EmailDomain> EmailDomains => Set<EmailDomain>();
    public DbSet<PhoneCode> PhoneCodes => Set<PhoneCode>();
    public DbSet<PersonEmail> PersonEmails => Set<PersonEmail>();
    public DbSet<PersonPhone> PersonPhones => Set<PersonPhone>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<PassengerType> PassengerTypes => Set<PassengerType>();
    public DbSet<Passenger> Passengers => Set<Passenger>();

    // Airlines
    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<AirportAirline> AirportAirlines => Set<AirportAirline>();

    // Staff
    public DbSet<StaffRole> StaffRoles => Set<StaffRole>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<AvailabilityStatus> AvailabilityStatuses => Set<AvailabilityStatus>();
    public DbSet<StaffAvailability> StaffAvailabilities => Set<StaffAvailability>();

    // Aircraft
    public DbSet<AircraftManufacturer> AircraftManufacturers => Set<AircraftManufacturer>();
    public DbSet<AircraftModel> AircraftModels => Set<AircraftModel>();
    public DbSet<AircraftUnit> AircraftUnits => Set<AircraftUnit>();
    public DbSet<CabinType> CabinTypes => Set<CabinType>();
    public DbSet<CabinConfiguration> CabinConfigurations => Set<CabinConfiguration>();

    // Routes
    public DbSet<Domain.Entities.Routes.Route> Routes => Set<Domain.Entities.Routes.Route>();
    public DbSet<RouteStop> RouteStops => Set<RouteStop>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Fare> Fares => Set<Fare>();

    // Flights
    public DbSet<FlightState> FlightStates => Set<FlightState>();
    public DbSet<FlightStatusTransition> FlightStatusTransitions => Set<FlightStatusTransition>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightRole> FlightRoles => Set<FlightRole>();
    public DbSet<FlightAssignment> FlightAssignments => Set<FlightAssignment>();
    public DbSet<SeatLocationType> SeatLocationTypes => Set<SeatLocationType>();
    public DbSet<FlightSeat> FlightSeats => Set<FlightSeat>();

    // Reservations
    public DbSet<ReservationStatus> ReservationStatuses => Set<ReservationStatus>();
    public DbSet<ReservationStatusTransition> ReservationStatusTransitions => Set<ReservationStatusTransition>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationFlight> ReservationFlights => Set<ReservationFlight>();
    public DbSet<ReservationPassenger> ReservationPassengers => Set<ReservationPassenger>();

    // Tickets
    public DbSet<TicketStatus> TicketStatuses => Set<TicketStatus>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<CheckInStatus> CheckInStatuses => Set<CheckInStatus>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();

    // Payments
    public DbSet<PaymentState> PaymentStates => Set<PaymentState>();
    public DbSet<PaymentMethodType> PaymentMethodTypes => Set<PaymentMethodType>();
    public DbSet<CardType> CardTypes => Set<CardType>();
    public DbSet<CardIssuer> CardIssuers => Set<CardIssuer>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Auth
    public DbSet<SystemRole> SystemRoles => Set<SystemRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}