using AirlineBookingSystem.Bookings.Application.Consumers;
using AirlineBookingSystem.Bookings.Application.Handlers;
using AirlineBookingSystem.Bookings.Core.Repositories;
using AirlineBookingSystem.Bookings.Infrastructure.Repositories;
using AirlineBookingSystem.BuildingBlocks.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;


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

//Redis
var redisConfiguration = builder.Configuration["CacheSettings:ConnectionString"];
var redis = ConnectionMultiplexer.Connect(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);	

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

//Add MassTransit
builder.Services.AddMassTransit(config =>
{
	// Register the consumer
	config.AddConsumer<NotificationEventConsumer>();

	// Register the bus and configure RabbitMQ
	config.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstant.NotificationSentQueue, e =>
		{
			e.ConfigureConsumer<NotificationEventConsumer>(context);
		});
	});
});



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
