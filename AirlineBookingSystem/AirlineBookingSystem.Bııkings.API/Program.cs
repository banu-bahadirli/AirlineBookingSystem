using AirlineBookingSystem.Bookings.Application.Handlers;
using AirlineBookingSystem.Bookings.Core.Repositories;
using AirlineBookingSystem.Bookings.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//Register MediatR
var assemblies = new Assembly[]
{
	Assembly.GetExecutingAssembly(),
	typeof(CreateBookingHandler).Assembly,
	typeof(GetBookingHandler).Assembly
};

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Add Sql Connection
builder.Services.AddScoped<IDbConnection>(provider =>
	new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
