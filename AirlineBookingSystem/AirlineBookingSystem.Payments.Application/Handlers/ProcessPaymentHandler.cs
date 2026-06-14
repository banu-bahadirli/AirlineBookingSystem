using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using AirlineBookingSystem.Payments.Application.Commands;
using AirlineBookingSystem.Payments.Core.Entities;
using AirlineBookingSystem.Payments.Core.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Payments.Application.Handlers
{
	public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, Guid>
	{

		private readonly IPaymentRepository _repository;
		private readonly IPublishEndpoint _publishEndpont;

		public ProcessPaymentHandler(IPaymentRepository repository,IPublishEndpoint publishEndpoint)
		{
			_repository = repository;
			_publishEndpont = publishEndpoint;
		}


		public async Task<Guid> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = new Payment
			{
				Id = Guid.NewGuid(),
				BookingId = request.BookingId,
				Amount = request.Amount,
				PaymentDate = DateTime.UtcNow

			};

			await _repository.ProcessPaymentAsync(payment);
			//Publish PaymentprocessEvent
			await _publishEndpont.Publish(new PaymentProcessEvent
			(
				payment.Id,
				payment.BookingId,
				payment.Amount,
				 payment.PaymentDate
			));

			return payment.Id;
		}
	}
}
