using Api.Extensions;
using Application.Abstractions;
using Application;
using Infrastructure;
using Infrastructure.Repositories.Addresses;
using Infrastructure.Repositories.Aircraft;
using Infrastructure.Repositories.AircraftManufacturers;
using Infrastructure.Repositories.AircraftModels;
using Infrastructure.Repositories.Airlines;
using Infrastructure.Repositories.AirportAirlines;
using Infrastructure.Repositories.Airports;
using Infrastructure.Repositories.AvailabilityStatuses;
using Infrastructure.Repositories.CabinConfigurations;
using Infrastructure.Repositories.CabinTypes;
using Infrastructure.Repositories.CheckIns;
using Infrastructure.Repositories.CheckInStatuses;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Clients;
using Infrastructure.Repositories.DocumentTypes;
using Infrastructure.Repositories.EmailDomains;
using Infrastructure.Repositories.Fares;
using Infrastructure.Repositories.FlightAssignments;
using Infrastructure.Repositories.Flights;
using Infrastructure.Repositories.FlightRoles;
using Infrastructure.Repositories.FlightSeats;
using Infrastructure.Repositories.FlightStates;
using Infrastructure.Repositories.FlightStatusTransitions;
using Infrastructure.Repositories.Invoices;
using Infrastructure.Repositories.Passengers;
using Infrastructure.Repositories.PassengerTypes;
using Infrastructure.Repositories.People;
using Infrastructure.Repositories.PersonEmails;
using Infrastructure.Repositories.PersonPhones;
using Infrastructure.Repositories.PhoneCodes;
using Infrastructure.Repositories.Regions;
using Infrastructure.Repositories.RoadTypes;
using Infrastructure.Repositories.Routes;
using Infrastructure.Repositories.RouteStops;
using Infrastructure.Repositories.ReservationStatuses;
using Infrastructure.Repositories.ReservationStatusTransitions;
using Infrastructure.Repositories.ReservationFlights;
using Infrastructure.Repositories.ReservationPassengers;
using Infrastructure.Repositories.Reservations;
using Infrastructure.Repositories.Seasons;
using Infrastructure.Repositories.SeatLocationTypes;
using Infrastructure.Repositories.Staff;
using Infrastructure.Repositories.StaffAvailabilities;
using Infrastructure.Repositories.StaffRoles;
using Infrastructure.Repositories.TicketStatuses;
using Infrastructure.Repositories.Tickets;
using Infrastructure.Repositories.InvoiceItemTypes;
using Infrastructure.Repositories.InvoiceItems;
using Infrastructure.Repositories.PaymentStates;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Add infrastructure services
builder.Services.ConfigureCors();
builder.Services.AddApplicationServices();
builder.Services.AddMapsterConfiguration();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IAircraftManufacturerRepository, AircraftManufacturerRepository>();
builder.Services.AddScoped<IAircraftModelRepository, AircraftModelRepository>();
builder.Services.AddScoped<IAirlineRepository, AirlineRepository>();
builder.Services.AddScoped<IAirportAirlineRepository, AirportAirlineRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAvailabilityStatusRepository, AvailabilityStatusRepository>();
builder.Services.AddScoped<ICabinConfigurationRepository, CabinConfigurationRepository>();
builder.Services.AddScoped<ICabinTypeRepository, CabinTypeRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<ICheckInStatusRepository, CheckInStatusRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IEmailDomainRepository, EmailDomainRepository>();
builder.Services.AddScoped<IFareRepository, FareRepository>();
builder.Services.AddScoped<IFlightAssignmentRepository, FlightAssignmentRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightRoleRepository, FlightRoleRepository>();
builder.Services.AddScoped<IFlightSeatRepository, FlightSeatRepository>();
builder.Services.AddScoped<IFlightStateRepository, FlightStateRepository>();
builder.Services.AddScoped<IFlightStatusTransitionRepository, FlightStatusTransitionRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IPassengerTypeRepository, PassengerTypeRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonEmailRepository, PersonEmailRepository>();
builder.Services.AddScoped<IPersonPhoneRepository, PersonPhoneRepository>();
builder.Services.AddScoped<IPhoneCodeRepository, PhoneCodeRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IReservationStatusRepository, ReservationStatusRepository>();
builder.Services.AddScoped<IReservationStatusTransitionRepository, ReservationStatusTransitionRepository>();
builder.Services.AddScoped<IReservationFlightRepository, ReservationFlightRepository>();
builder.Services.AddScoped<IReservationPassengerRepository, ReservationPassengerRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoadTypeRepository, RoadTypeRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteStopRepository, RouteStopRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<ISeatLocationTypeRepository, SeatLocationTypeRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffAvailabilityRepository, StaffAvailabilityRepository>();
builder.Services.AddScoped<IStaffRoleRepository, StaffRoleRepository>();
builder.Services.AddScoped<ITicketStatusRepository, TicketStatusRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceItemTypeRepository, InvoiceItemTypeRepository>();
builder.Services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
builder.Services.AddScoped<IPaymentStateRepository, PaymentStateRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
