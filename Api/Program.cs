using Api.Extensions;
using Application.Abstractions;
using Application;
using Infrastructure;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Regions;

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
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

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
