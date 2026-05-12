using Api.Extensions;
using Application.Abstractions;
using Application;
using Infrastructure;
using Infrastructure.Repositories.Addresses;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.DocumentTypes;
using Infrastructure.Repositories.People;
using Infrastructure.Repositories.Regions;
using Infrastructure.Repositories.RoadTypes;

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
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRoadTypeRepository, RoadTypeRepository>();

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
