using AirlineBookingSystem.Notifications.Application.Commands;
using AirlineBookingSystem.Notifications.Core.Entities;
using AirlineBookingSystem.Notifications.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Notifications.Application.Handlers
{
	public class SendNotificationHandler : IRequestHandler<SendNotificationCommand>
	{
		private readonly INotificationRepository _notificationRepository;

		public SendNotificationHandler(INotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}
		public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
		{
			var notification = new Notification
			{
				Id = Guid.NewGuid(),
				Recipient = request.Recepient,
				Message = request.Message,
				Type= request.Type
			};

			await _notificationRepository.LogNotificationAsync(notification);
		}
	}
}
