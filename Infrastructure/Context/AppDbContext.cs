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
    // Geography
    public DbSet<Continent> Continents { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
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
    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<PassengerType> PassengerTypes { get; set; } = default!;
    public DbSet<Passenger> Passengers { get; set; } = default!;

    // Airlines
    public DbSet<Airline> Airlines { get; set; } = default!;
    public DbSet<Airport> Airports { get; set; } = default!;
    public DbSet<AirportAirline> AirportAirlines { get; set; } = default!;

    // Staff
    public DbSet<StaffRole> StaffRoles { get; set; } = default!;
    public DbSet<StaffMember> StaffMembers { get; set; } = default!;
    public DbSet<AvailabilityStatus> AvailabilityStatuses { get; set; } = default!;
    public DbSet<StaffAvailability> StaffAvailabilities { get; set; } = default!;

    // Aircraft
    public DbSet<AircraftManufacturer> AircraftManufacturers { get; set; } = default!;
    public DbSet<AircraftModel> AircraftModels { get; set; } = default!;
    public DbSet<AircraftUnit> AircraftUnits { get; set; } = default!;
    public DbSet<CabinType> CabinTypes { get; set; } = default!;
    public DbSet<CabinConfiguration> CabinConfigurations { get; set; } = default!;

    // Routes
    public DbSet<Domain.Entities.Routes.Route> Routes { get; set; } = default!;
    public DbSet<RouteStop> RouteStops { get; set; } = default!;
    public DbSet<Season> Seasons { get; set; } = default!;
    public DbSet<Fare> Fares { get; set; } = default!;

    // Flights
    public DbSet<FlightState> FlightStates { get; set; } = default!;
    public DbSet<FlightStatusTransition> FlightStatusTransitions { get; set; } = default!;
    public DbSet<Flight> Flights { get; set; } = default!;
    public DbSet<FlightRole> FlightRoles { get; set; } = default!;
    public DbSet<FlightAssignment> FlightAssignments { get; set; } = default!;
    public DbSet<SeatLocationType> SeatLocationTypes { get; set; } = default!;
    public DbSet<FlightSeat> FlightSeats { get; set; } = default!;

    // Reservations
    public DbSet<ReservationStatus> ReservationStatuses { get; set; } = default!;
    public DbSet<ReservationStatusTransition> ReservationStatusTransitions { get; set; } = default!;
    public DbSet<Reservation> Reservations { get; set; } = default!;
    public DbSet<ReservationFlight> ReservationFlights { get; set; } = default!;
    public DbSet<ReservationPassenger> ReservationPassengers { get; set; } = default!;

    // Tickets
    public DbSet<TicketStatus> TicketStatuses { get; set; } = default!;
    public DbSet<Ticket> Tickets { get; set; } = default!;
    public DbSet<CheckInStatus> CheckInStatuses { get; set; } = default!;
    public DbSet<CheckIn> CheckIns { get; set; } = default!;

    // Payments
    public DbSet<PaymentState> PaymentStates { get; set; } = default!;
    public DbSet<PaymentMethodType> PaymentMethodTypes { get; set; } = default!;
    public DbSet<CardType> CardTypes { get; set; } = default!;
    public DbSet<CardIssuer> CardIssuers { get; set; } = default!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = default!;
    public DbSet<Payment> Payments { get; set; } = default!;

    // Auth
    public DbSet<SystemRole> SystemRoles { get; set; } = default!;
    public DbSet<Permission> Permissions { get; set; } = default!;
    public DbSet<RolePermission> RolePermissions { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Session> Sessions { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
