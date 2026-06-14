using AirlineBookingSystem.BuildingBlocks.Common;
using AirlineBookingSystem.Notifications.Application.Consumers;
using AirlineBookingSystem.Notifications.Application.Handlers;
using AirlineBookingSystem.Notifications.Application.Interfaces;
using AirlineBookingSystem.Notifications.Application.Services;
using AirlineBookingSystem.Notifications.Core.Repositories;
using AirlineBookingSystem.Notifications.Infrastructure.Repositories;
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
	typeof(SendNotificationHandler).Assembly
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
//Application Servies
builder.Services.AddScoped<INotificationService, NotificationService>();
//Application Servies
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
//Add MassTransit
builder.Services.AddMassTransit(config =>
{
	// Register the consumer
	config.AddConsumer<PaymentProcessedConsumer>();

	// Register the bus and configure RabbitMQ
	config.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstant.PaymentProcessQueue, e =>
		{
			e.ConfigureConsumer<PaymentProcessedConsumer>(context);
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
