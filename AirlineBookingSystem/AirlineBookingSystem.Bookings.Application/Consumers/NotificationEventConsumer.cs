using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using MassTransit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Bookings.Application.Consumers
{
	public class NotificationEventConsumer : IConsumer<NotificationEvent>
	{
		public async Task Consume(ConsumeContext<NotificationEvent> context)
		{
			var notificationEvent = context.Message;
			Console.WriteLine($"Received notification for {notificationEvent.Recipient}: {notificationEvent.Message} (Type: {notificationEvent.Type})");
		    await Task.CompletedTask;

		}
	}
}
