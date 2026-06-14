using AirlineBookingSystem.BuildingBlocks.Common;
using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using AirlineBookingSystem.Payments.Application.Consumers;
using AirlineBookingSystem.Payments.Application.Handlers;
using AirlineBookingSystem.Payments.Core.Repositories;
using AirlineBookingSystem.Payments.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Data.SqlClient;
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
	typeof(ProcessPaymentHandler).Assembly,
	typeof(RefundPaymentHandler).Assembly
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
//Application Servies
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


//Application Servies
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddMassTransit(config =>
{
	// Register the consumer
	config.AddConsumer<FlightBookedConsumer>();

	// Register the bus and configure RabbitMQ
	config.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstant.FlightBookedQueue, e =>
		{
			e.ConfigureConsumer<FlightBookedConsumer>(context);
		});
	});
});

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
