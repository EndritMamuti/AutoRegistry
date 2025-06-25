using AutoRegistry.Core.Interfaces;
using AutoRegistry.Core.Services;
using AutoRegistry.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Use traditional API controllers
builder.Services.AddEndpointsApiExplorer(); // Enables OpenAPI
builder.Services.AddSwaggerGen(); // Adds Swagger UI

// Register application services
builder.Services.AddScoped<IVehicleRepository, InMemoryVehicleRepository>();
builder.Services.AddScoped<VehicleInspectionService>();

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Enable attribute-based routing

app.Run();