using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using AirlineBookingSystem.Notifications.Application.Commands;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Notifications.Application.Consumers
{
	public class PaymentProcessedConsumer : IConsumer<PaymentProcessEvent>
	{
		private readonly IMediator _mediator;

		public PaymentProcessedConsumer(IMediator mediator)
		{
			_mediator = mediator;
		}
		public async Task Consume(ConsumeContext<PaymentProcessEvent> context)
		{
			var paymentProcessedEvent = context.Message;
			var message = $"Payment of $: {paymentProcessedEvent.Amount}, for Booking ID: {paymentProcessedEvent.BookingId}, was processed successfully.";
			var command = new SendNotificationCommand("someone@example.com", message, "Email");
			await _mediator.Send(command);

		}
	}
}