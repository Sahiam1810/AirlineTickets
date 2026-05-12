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
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Clients;
using Infrastructure.Repositories.DocumentTypes;
using Infrastructure.Repositories.EmailDomains;
using Infrastructure.Repositories.Fares;
using Infrastructure.Repositories.FlightStates;
using Infrastructure.Repositories.PassengerTypes;
using Infrastructure.Repositories.People;
using Infrastructure.Repositories.PersonEmails;
using Infrastructure.Repositories.PersonPhones;
using Infrastructure.Repositories.PhoneCodes;
using Infrastructure.Repositories.Regions;
using Infrastructure.Repositories.RoadTypes;
using Infrastructure.Repositories.Routes;
using Infrastructure.Repositories.RouteStops;
using Infrastructure.Repositories.Seasons;
using Infrastructure.Repositories.Staff;
using Infrastructure.Repositories.StaffAvailabilities;
using Infrastructure.Repositories.StaffRoles;

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
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IEmailDomainRepository, EmailDomainRepository>();
builder.Services.AddScoped<IFareRepository, FareRepository>();
builder.Services.AddScoped<IFlightStateRepository, FlightStateRepository>();
builder.Services.AddScoped<IPassengerTypeRepository, PassengerTypeRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonEmailRepository, PersonEmailRepository>();
builder.Services.AddScoped<IPersonPhoneRepository, PersonPhoneRepository>();
builder.Services.AddScoped<IPhoneCodeRepository, PhoneCodeRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRoadTypeRepository, RoadTypeRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteStopRepository, RouteStopRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffAvailabilityRepository, StaffAvailabilityRepository>();
builder.Services.AddScoped<IStaffRoleRepository, StaffRoleRepository>();

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
